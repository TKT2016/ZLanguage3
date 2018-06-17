using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Parsers.Raws;

namespace ZCompileCore.AST
{
    public class StmtBlock:Stmt
    {
        private List<Stmt> Stmts;
        private ProcAST _Proc;
        public StmtBlockRaw Raw;
         
        public StmtBlock(ProcAST proc, StmtBlockRaw raw)
        {
            _Proc = proc;
            Raw = raw;
            Stmts = new List<Stmt>();
            this._ProcContext = proc.GetContextProc();
        }

        public StmtBlock(Stmt parentStmt, StmtBlockRaw raw)
        {          
            ParentStmt = parentStmt;
            Raw = raw;
            Stmts = new List<Stmt>();
        }

        public override void AnalyExpDim()
        {
            foreach (Stmt stmt in Stmts)
            {
                stmt.AnalyExpDim();
            }
        }

        public override Stmt Analy()
        {
            Stmts.Clear();
            
            StmtRawParser parser = new StmtRawParser(Raw.LineTokens, this.ProcContext);
            while (parser.HasCurrent)
            {
                object obj = parser.ParseCurrent();
                if(obj!=null)
                {
                    if(obj is StmtRaw)
                    {
                        //Console.WriteLine(obj.ToString());
                        AnalyRaw((StmtRaw)obj);
                    }
                    else if (obj is List<StmtRaw>)
                    {
                        List<StmtRaw> raws = (List<StmtRaw>)obj;
                        foreach (StmtRaw item in raws)
                        {
                            //Console.WriteLine(item.ToString());
                            //if(item.ToString().StartsWith("结果=否"))
                            //{
                            //    //Console.WriteLine("debug:"+item.ToString());
                            //    Debugr.IsInDebug = true;
                            //}
                            AnalyRaw(item);
                        }
                    }
                    else
                    {
                        throw new CCException();
                    }
                }
            }
 
            return this;
        }

        private void AnalyRaw(StmtRaw raw)
        {
            Stmt stmt = CreateStmt((StmtRaw)raw);
            Stmt stmt2 = stmt.Analy();
            Stmts.Add(stmt2);
        }

        public override void Emit()
        {
            foreach (Stmt stmt in Stmts)
            {
                stmt.Emit();
            }
        }

        private Stmt CreateStmt(StmtRaw raw)
        {
            if(raw is StmtCallRaw)
            {
                StmtCall stmt = new StmtCall((StmtCallRaw)raw,this);
                return stmt;
            }
            else if (raw is StmtCatchRaw)
            {
                StmtCatch stmt = new StmtCatch((StmtCatchRaw)raw, this);
                return stmt;
            }
            else if (raw is StmtForeachRaw)
            {
                StmtForeach stmt = new StmtForeach((StmtForeachRaw)raw, this);
                return stmt;
            }
            else if (raw is StmtIfRaw)
            {
                StmtIf stmt = new StmtIf((StmtIfRaw)raw, this);
                return stmt;
            }
            else if (raw is StmtRepeatRaw)
            {
                StmtRepeat stmt = new StmtRepeat((StmtRepeatRaw)raw, this);
                return stmt;
            }
            else if (raw is StmtWhileRaw)
            {
                StmtWhile stmt = new StmtWhile((StmtWhileRaw)raw, this);
                return stmt;
            }
            else
            {
                throw new CCException();
            }
        }

        public override string ToString()
        {
            return Raw.ToString();
        } 
    }
}
