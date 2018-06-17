using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Models.Lang
{
    public class TKTClassModel : TKTModelBase
    {
        public TKTContentModel NameModel { get; set; }
        public List<TKTContentModel> UsingPackages { get; private set; }
        //public List<TKTContentModel> UsingTypes { get; private set; }
        public List<TKTContentModel> RedirectTypes { get; private set; }
  
        public List<TKTPropertyModel> PropertyList { get; private set; }
        public List<TKTContentModel> EnumItems { get; private set; }
        public List<TKTConstructionModel> ContructList { get; private set; }
        public List<TKTProcModel> ProcList { get; private set; }

        //static TKTContentModel ObjectBase = new TKTContentModel() { Content = "事物" };
        TKTContentModel _BaseType;

        public TKTContentModel BaseType
        {
            get
            {
                if (_BaseType == null )
                    _BaseType = new TKTContentModel() { Content = "事物" }; ;
                if (_BaseType!=null && string.IsNullOrEmpty(_BaseType.Content))
                    _BaseType.Content= "事物";
                return _BaseType;
            }
            set
            {
                _BaseType = value;
            }
        }

        public TKTClassModel()
        {
            UsingPackages = new List<TKTContentModel>();
            //UsingTypes = new List<TKTContentModel>();
            RedirectTypes = new List<TKTContentModel>();
            PropertyList = new List<TKTPropertyModel>();
            ContructList = new List<TKTConstructionModel>();
            ProcList = new List<TKTProcModel>();
            EnumItems = new List<TKTContentModel>();
        }

        public string GetTypeName()
        {
            if (NameModel == null) return null;
            return NameModel.Content;
        }
    }
}
