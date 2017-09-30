using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZLangRT.Attributes;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class SectionClassName: SectionBase
    {
        public Token BaseTypeToken;
        public Token NameToken;

        public string ClassName;
        public bool IsStatic;
        public ZClassType BaseZType;
        public string BaseTypeName;

        public void Analy(string fileName, ContextClass context)
        {
            if (NameToken == null)
            {
                ClassName = fileName;
            }
            else
            {
                ClassName = NameToken.GetText();
                if(ClassName!=fileName)
                {
                    this.FileContext.Errorf(BaseTypeToken.Position, "类名称 '" + ClassName + "'和文件名称'" + fileName + "'不一致");
                }
            }
            AnalyBase(context);
        }
        public SuperSymbolTable SuperTable { get; protected set; }
        private void AnalyBase(ContextClass context)
        {
            SuperTable = null;
            if (BaseTypeToken == null)
            {
                IsStatic = false;
                BaseZType = ZLangBasicTypes.ZOBJECT;
                SuperTable = new SuperSymbolTable("SUPER-唯一", ZLangBasicTypes.ZOBJECT);
            }
            else
            {
                BaseTypeName = BaseTypeToken.GetText();
                if (BaseTypeName == "唯一")
                {
                    IsStatic = true;
                    SuperTable = new SuperSymbolTable("SUPER-唯一", null);
                }
                else
                {
                    IsStatic = false;
                    var ztypes = ZTypeManager.GetByMarkName(BaseTypeName);
                    if (ztypes.Length == 0)
                    {
                        errorf(BaseTypeToken.Position, "类型'{0}'不存在", BaseTypeName);
                    }
                    else if(ztypes.Length==1)
                    {
                        BaseZType = ztypes[0] as ZClassType;
                        SuperTable = new SuperSymbolTable("SUPER-" + BaseZType.ZName, BaseZType);
                    }
                    else if(ztypes.Length==0)
                    {
                        errorf(BaseTypeToken.Position, "'{0}'存在{1}个同名类型", BaseTypeName, ztypes.Length);
                        SuperTable = new SuperSymbolTable("SUPER", null);
                    }
                }
            }
            if (SuperTable==null)
            {
                SuperTable = new SuperSymbolTable("SUPER-STATIC", null);
            }
            SuperTable.ParentTable = FileContext.UseContext.SymbolTable;
            context.SetSuperTable(SuperTable);
            context.BaseZType = BaseZType;
            
        }

        public TypeBuilder Builder;
        public ZType EmitedZType;

        public TypeBuilder Emit(ModuleBuilder moduleBuilder,string packageName)
        {
            var fullName = packageName + "." + ClassName;
            TypeAttributes typeAttrs = TypeAttributes.Public;
            if(IsStatic)
                typeAttrs = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;
            Builder = null;
            if(BaseZType!=null)
            {
                Builder = moduleBuilder.DefineType(fullName, typeAttrs, BaseZType.SharpType);
            }
            else
            {
                Builder = moduleBuilder.DefineType(fullName, typeAttrs);
            }
            SetAttr(Builder);
            return Builder;
        }

        protected void SetAttr(TypeBuilder classBuilder)
        {
            Type myType = IsStatic ? typeof(ZStaticAttribute) : typeof(ZInstanceAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(BaseTypeToken.GetText());
            buf.Append("类型");
            buf.Append(":");
            buf.Append(NameToken.GetText());
            return buf.ToString();
        }
    }
}
