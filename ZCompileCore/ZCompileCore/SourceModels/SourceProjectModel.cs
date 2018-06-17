using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.SourceModels
{
    public class SourceProjectModel
    {
        /// <summary>
        /// 引用的外部DLL
        /// </summary>
        public List<FileInfo> RefDllList { get;private set; }

        /// <summary>
        /// 引用的包名称
        /// </summary>
        public List<string> RefPackageList { get; private set; }

        /// <summary>
        /// 项目包名称
        /// </summary>
        public string ProjectPackageName { get; set; }

        /// <summary>
        /// 生成的文件类型
        /// </summary>
        public PEFileKinds BinaryFileKind { get; set; }

        /// <summary>
        /// 生成的文件名称(不带扩展名)
        /// </summary>
        public string BinaryFileNameNoEx { get; set; }

        /// <summary>
        /// 编译后的保存二进制文件的文件夹
        /// </summary>
        public DirectoryInfo BinarySaveDirectoryInfo { get; set; }

        /// <summary>
        /// 入口类名称
        /// </summary>
        public string EntryClassName { get; set; }

        /// <summary>
        /// 项目程序文件信息列表
        /// </summary>
        public List<SourceFileModel> SouceFileList { get; set; }

        /// <summary>
        /// 项目根目录
        /// </summary>
        public DirectoryInfo ProjectRootDirectoryInfo { get; set; }

        /// <summary>
        /// 项目文件信息
        /// </summary>
        public string ProjectFilePath{ get; set; }

        /// <summary>
        /// 是否保存生成的二进制文件
        /// </summary>
        public bool NeedSave { get; set; }

        public SourceProjectModel()
        {
            RefDllList = new List<FileInfo>();
            RefPackageList = new List<string>();
            SouceFileList = new List<SourceFileModel>();
        }

        public void AddFile(SourceFileModel zCompileClassModel)
        {
            SouceFileList.Add(zCompileClassModel);
        }

        public void AddRefDll(FileInfo dll)
        {
            RefDllList.Add(dll);
        }

        public void AddRefPackage(params string[] packageNames)
        {
            RefPackageList.AddRange(packageNames);
        }

        public string GetBinaryNameEx()
        {
            string binFileName = this.BinaryFileNameNoEx;
            if (this.BinaryFileKind == PEFileKinds.Dll)
            {
                binFileName += ".dll";
            }
            else
            {
                binFileName += ".exe";
            }
            return binFileName;
        }
    }
}
