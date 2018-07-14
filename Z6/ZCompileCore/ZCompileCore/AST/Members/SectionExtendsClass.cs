using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class SectionExtendsClass
    {
        public SectionExtendsRaw Raw;
        private ClassAST ASTClass;
        public string ExtendsTypeName { get;private set; }
         
        public SectionExtendsClass(ClassAST classAST, SectionExtendsRaw raw)
        {
            ASTClass = classAST;
            Raw = raw;
        }

        public void Analy()
        {
            string fileName = this.ASTClass.FileContext.FileModel.GeneratedClassName;
            bool IsStatic = false;
            ZLClassInfo BaseZType = null;
            if(Raw==null)
            {
                IsStatic = false;
                BaseZType = ZLangBasicTypes.ZOBJECT;
            }
            else if (Raw.BaseTypeToken == null)
            {
                ExtendsTypeName = null;
                IsStatic = false;
                BaseZType = ZLangBasicTypes.ZOBJECT;
            }
            else
            {
                ExtendsTypeName = Raw.BaseTypeToken.Text;
                if (ExtendsTypeName == "唯一类型")
                {
                    IsStatic = true;
                }
                else if (ExtendsTypeName == "普通类型" || ExtendsTypeName == "一般类型")
                {
                    IsStatic = false;
                    BaseZType = ZLangBasicTypes.ZOBJECT;
                }
                else
                {
                    IsStatic = false;
                    var ztypes = ZTypeManager.GetByMarkName(ExtendsTypeName);
                    if (ztypes.Length == 0)
                    {
                        this.ASTClass.FileContext.Errorf(Raw.BaseTypeToken.Position, "类型'{0}'不存在", ExtendsTypeName);
                    }
                    else if (ztypes.Length == 1)
                    {
                        BaseZType = ztypes[0] as ZLClassInfo;
                    }
                    else if (ztypes.Length == 0)
                    {
                        this.ASTClass.FileContext.Errorf(Raw.BaseTypeToken.Position, "'{0}'存在{1}个同名类型", ExtendsTypeName, ztypes.Length.ToString());
                    }
                }
                //if (ClassName != fileName)
                //{
                //    this.FileContext.Errorf(BaseTypeToken.Position, "类名称 '" + ClassName + "'和文件名称'" + fileName + "'不一致");
                //}
            }
            this.ASTClass.ClassContext.SetIsStatic(IsStatic);
            this.ASTClass.ClassContext.SetSuperClass(BaseZType);
        }

        public override string ToString()
        {
            if (Raw != null)
                return Raw.ToString();
            else
                return "属于:";
        }
    }
}
