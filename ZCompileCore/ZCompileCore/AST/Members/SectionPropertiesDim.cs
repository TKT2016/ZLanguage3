using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Lex;
using ZCompileCore.Tools;

namespace ZCompileCore.AST
{
    public class SectionPropertiesDim
    {
        public SectionPropertiesRaw Raw;
        private DimAST dimAST;
        private List<PropertyAST> PropertyASTList = new List<PropertyAST>();
        internal Dictionary<string, PropertyAST> dict = new Dictionary<string, PropertyAST>();
        public ConstructorBuilder constructorBuilder { get; private set; }

        public SectionPropertiesDim(DimAST dimAST, SectionPropertiesRaw raw)
        {
            this.dimAST = dimAST;
            Raw = raw;

            for (int i = 0; i < Raw.Properties.Count; i++)
            {
                PropertyASTRaw propertyRaw = Raw.Properties[i];
                PropertyAST propertyAST = new PropertyAST(propertyRaw, this);
                PropertyASTList.Add(propertyAST);
            }
        }

        public void AnalyNames()
        {
            dict.Clear();
            for (int i = 0; i < PropertyASTList.Count; i++)
            {
                PropertyAST propertyAST = PropertyASTList[i];
                propertyAST.AnalyName();
               
            }
        }

        public void AnalyTypes()
        {
            dict.Clear();
            for (int i = 0; i < PropertyASTList.Count; i++)
            {
                PropertyAST propertyAST = PropertyASTList[i];
                propertyAST.AnalyType();
            }
        }

        public void EmitNames()
        {
            for (int i = 0; i < PropertyASTList.Count; i++)
            {
                PropertyAST propertyAST = PropertyASTList[i];
                propertyAST.EmitName();
            }
        }
      
        public void EmitBodys()
        {
            TypeBuilder dimBuilder = this.dimAST.DimBuilder;
            
            constructorBuilder = dimBuilder.DefineConstructor(
            MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, new Type[] { });
            ILGenerator IL = constructorBuilder.GetILGenerator();

            //foreach (var item in DimItems)
            //{
            //    //item.Emit(Builder, IL);
            //    FieldAttributes fieldAttr = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
            //    var fieldBuilder = dimBuilder.DefineField(item.DimName, typeof(string), fieldAttr);
            //    EmitHelper.LoadString(IL, item.DimType);
            //    EmitHelper.StormField(IL, fieldBuilder);
            //}
            for (int i = 0; i < PropertyASTList.Count; i++)
            {
                PropertyAST propertyAST = PropertyASTList[i];
                propertyAST.EmitBody();
            }
            IL.Emit(OpCodes.Ret);
        }

        public class PropertyAST
        {
            private PropertyASTRaw Raw;
            private SectionPropertiesDim ParentAST;
            public string DimName { get; private set; }
            public string DimType { get; private set; }
            private bool IsContains;
            private FieldBuilder fieldBuilder;

            public PropertyAST(PropertyASTRaw raw,SectionPropertiesDim parentAST)
            {
                Raw = raw;
                ParentAST = parentAST;
            }

            public void AnalyName()
            {
                DimName = Raw.NameToken.Text;
                IsContains = this.ParentAST.dict.ContainsKey(DimName);
                if (IsContains)//this.dict.ContainsKey(dimName))
                {
                    Errorf(Raw.NameToken.Position, "'{0}'重复声明", DimName);
                }
                else
                {
                    this.ParentAST.dict.Add(DimName, this);
                    //this.FileContext.ImportUseContext.AddDim(DimName, DimTypeName);
                }
            }

            public void AnalyType()
            {
                if (IsContains) return;
                var ValueExp = Raw.ValueExp;
                if (ValueExp == null)
                {
                    Errorf(Raw.NameToken.Position, "'{0}'没有声明类型", DimName);
                }
                else if (ValueExp.RawTokens.Count == 0)
                {
                    Errorf(Raw.NameToken.Position, "'{0}'没有声明类型", DimName);
                }
                else
                {
                    LexTokenText typeToken = GetDimTypeToken(ValueExp);
                    if (typeToken == null)
                    {
                        Errorf(Raw.NameToken.Position, "'{0}'声明的不是类型", DimName);
                    }
                    else
                    {
                        DimType = typeToken.Text;
                    }
                }
            }

            public void EmitName()
            {
                TypeBuilder dimBuilder = this.ParentAST.dimAST.DimBuilder;
                FieldAttributes fieldAttr = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
                fieldBuilder = dimBuilder.DefineField(DimName, typeof(string), fieldAttr);
            }

            public void EmitBody()
            {
                var constructorBuilder = this.ParentAST.constructorBuilder;
                ILGenerator IL = constructorBuilder.GetILGenerator();
                EmitHelper.LoadString(IL, DimType);
                EmitHelper.StormField(IL, fieldBuilder);
            }

            private LexTokenText GetDimTypeToken(ExpRaw ValueExp)
            {
                if (ValueExp == null) return null;
                if (ValueExp.RawTokens.Count == 0) return null;
                LexToken token = ValueExp.RawTokens[0];
                if (!(token is LexTokenText)) return null;
                LexTokenText result = (LexTokenText)token;
                return result;
            }

            private void Errorf(CodePosition position, string messagef, params object[] args)
            {
                this.ParentAST.dimAST.FileContext.Errorf(position, messagef, args);
            }
        }
    }
}
