using System;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Parsers.Raws;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ClassPropertyAST
    {
        private SectionPropertiesClass ParentProperties;
        private PropertyASTRaw Raw;
        public string PropertyName;
        private ZCPropertyInfo ZPropertyCompiling;
        private ZType PropertyZType;
        private Exp PropertyValueExp;
        private ContextExp ExpContext;
        public bool IsExists { get; private set; }

        public ClassPropertyAST(SectionPropertiesClass sectionProperties, PropertyASTRaw raw)
        {
            ParentProperties = sectionProperties;
            Raw = raw;
            ExpContext = new ContextExp(this.ParentProperties.PropertiesContext, null);
        }
        
        public void AnalyName()
        {
            ZPropertyCompiling = new ZCPropertyInfo();
            PropertyName = Raw.NameToken.Text;
            if (ClassContext.ContainsPropertyName(PropertyName))// (ParentProperties.dict.ContainsKey(PropertyName))
            {
                this.ParentProperties.ASTClass.FileContext.Errorf(Raw.NameToken.Position, "'{0}'重复", PropertyName);
                IsExists = true;
            }
            else
            {
                //ParentProperties.dict.Add(PropertyName, Raw.NameToken);
                ZPropertyCompiling.ZPropertyZName = PropertyName;
                ClassContext.AddMember(ZPropertyCompiling);
                //AnalyType();
                //PropertyItems.Add(name);
            }
        }

        private ContextClass ClassContext
        {
            get
            {
                return this.ParentProperties.ASTClass.ClassContext;
            }
        }

        public void AnalyType()
        {
            if (IsExists) return;
            PropertyZType = AnalyPropertyZType();
            ZPropertyCompiling.ZPropertyType = PropertyZType;//.SetMemberType(PropertyZType);
        }

        private bool HasValue()
        {
            return this.Raw.ValueExp != null;
        }

        private ZType AnalyPropertyZType()
        {
            
            if (IsExists) return null;
            bool isStatic = this.ParentProperties.ASTClass.ClassContext.IsStatic();
            ZPropertyCompiling.IsStatic = isStatic;
            ContextImportUse importUseContext = this.ParentProperties.ASTClass.FileContext.ImportUseContext;
            if (HasValue())
            {
                PropertyValueExp = AnalyPropertyValueExp();
                ZType ztype = PropertyValueExp.RetType;
                if (ztype != null)
                {
                    return ztype;
                }
            }
            else
            {
                //if (Raw.NameToken.Text.StartsWith("子弹类型"))
                //{
                //    Debugr.WriteLine("子弹类型");
                //}
                ZType[] ztypes = importUseContext.SearchImportType(this.PropertyName);
                if (ztypes.Length == 1)
                {
                    return ztypes[0];
                }
                else if (ztypes.Length == 0)
                {
                    this.ExpContext.FileContext.Errorf(Raw.NameToken.Line,Raw.NameToken.Col, "没有搜索到属性‘" + PropertyName + "’的类型");
                }
                else
                {

                }
            } 
            return ZLangBasicTypes.ZOBJECT;
        }

        public void AnalyExpDim()
        {
            if (PropertyValueExp!=null)
                PropertyValueExp.AnalyDim();
        }

        private Exp AnalyPropertyValueExp()
        {
            ExpRawParser parser = new ExpRawParser();
            Exp exp1 = parser.Parse(Raw.ValueExp, ExpContext);
            Exp exp2 = exp1.Analy();
            return exp2;
        }

        private FieldBuilder _fieldBuilder;
        private PropertyBuilder _propertyBuilder;

        public void EmitName()
        {
            bool isStatic = this.ParentProperties.ASTClass.ClassContext.IsStatic();
            var classBuilder = this.ParentProperties.ASTClass.ClassContext.GetTypeBuilder();
            ZPropertyCompiling.IsStatic = isStatic;
            Type propertyType = null;
            if (PropertyZType is ZLType)
            {
                propertyType = ((ZLType)PropertyZType).SharpType;
            }
            else
            {
                propertyType = ((ZCClassInfo)PropertyZType).ClassBuilder;
            }
            MethodAttributes methodAttr = BuilderUtil.GetMethodAttr(isStatic);
            FieldAttributes fieldAttr = BuilderUtil.GetFieldAttr(isStatic);

            _fieldBuilder = classBuilder.DefineField("_" + PropertyName, propertyType, fieldAttr);
            _propertyBuilder = classBuilder.DefineProperty(PropertyName, PropertyAttributes.None, propertyType, null);

            EmitGet(classBuilder, PropertyName, propertyType, isStatic, _fieldBuilder, _propertyBuilder, methodAttr);
            EmitSet(classBuilder, PropertyName, propertyType, isStatic, _fieldBuilder, _propertyBuilder, methodAttr);
            ASTUtil.SetZAttr(_propertyBuilder, PropertyName);
            ZPropertyCompiling.PropertyBuilder = _propertyBuilder;
        }

        private void EmitGet(TypeBuilder classBuilder, string propertyName, Type propertyType, bool isStatic, FieldBuilder field, PropertyBuilder property, MethodAttributes methodAttr)
        {
            MethodBuilder methodGet = classBuilder.DefineMethod("get_" + propertyName, methodAttr, propertyType, null);
            var ilget = methodGet.GetILGenerator();
            EmitHelper.EmitThis(ilget, isStatic);
            LocalBuilder retBuilder = ilget.DeclareLocal(propertyType);
            EmitHelper.LoadField(ilget, field);
            EmitHelper.StormVar(ilget, retBuilder);
            EmitHelper.LoadVar(ilget, retBuilder);
            ilget.Emit(OpCodes.Ret);
            property.SetGetMethod(methodGet);
        }

        private void EmitSet(TypeBuilder classBuilder, string propertyName, Type propertyType, bool isStatic, FieldBuilder field, PropertyBuilder property, MethodAttributes methodAttr)
        {
            MethodBuilder methodSet = classBuilder.DefineMethod("set_" + propertyName, methodAttr, typeof(void), new Type[] { propertyType });
            ILGenerator ilSet = methodSet.GetILGenerator();
            EmitHelper.EmitThis(ilSet, isStatic);
            if (isStatic)
            {
                ilSet.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                ilSet.Emit(OpCodes.Ldarg_1);
            }

            EmitHelper.StormField(ilSet, field);
            ilSet.Emit(OpCodes.Ret);
            property.SetSetMethod(methodSet);
        }

        public void EmitBody()
        {
            if (IsExists) return;

            var classContext = this.ParentProperties.ASTClass.ClassContext;
            var method = classContext.InitPropertyMethod;
            var IL = method.GetILGenerator();
            bool isStatic = classContext.IsStatic();
            if (!isStatic)
            {
                IL.Emit(OpCodes.Ldarg_0);
            }
            if (HasValue())
            {
                PropertyValueExp.Emit();
            }
            else
            {
                Type type = ZTypeUtil.GetTypeOrBuilder( PropertyZType);
                EmitHelper.LoadDefaultValue(IL,type);
            }
            EmitHelper.StormField(IL, _fieldBuilder);
        }
    }
}
