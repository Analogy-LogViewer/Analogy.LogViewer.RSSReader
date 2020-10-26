﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using Analogy.LogViewer.RSSReader.Core;
using Analogy.LogViewer.Template;

namespace Analogy.LogViewer.RSSReader
{
    public class DownloadInformation : Analogy.LogViewer.Template.AnalogyDownloadInformation
    {
        protected override string RepositoryURL { get; set; } = "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.RSSReader";
        public override TargetFrameworkAttribute CurrentFrameworkAttribute { get; set; } = (TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(TargetFrameworkAttribute));

        public override Guid FactoryId { get; set; } = RSSFactory.Id;
        public override string Name { get; set; } = "Affirmations Data Provider";

        private string? _installedVersionNumber;
        public override string InstalledVersionNumber
        {
            get
            {
                if (_installedVersionNumber != null)
                {
                    return _installedVersionNumber;
                }
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                _installedVersionNumber = fvi.FileVersion;
                return _installedVersionNumber;
            }
        }
    }
}
