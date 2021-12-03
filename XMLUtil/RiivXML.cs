﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;

namespace XMLUtil
{
    public sealed class RiivXML
    {
        public XmlDocument Document;

        public Options options { get; internal set; }

        public List<Patch> Patches { get; internal set; }

        public RiivXML()
        {
            Document = new XmlDocument();
            var node = Document.CreateElement("wiidisk");
            var attribute = Document.CreateAttribute("version");
            attribute.Value = "1";
            node.Attributes.Append(attribute);
            Document.AppendChild(node);
            options = new Options
            {
                Sections = new List<Section>()
            };
            Patches = new List<Patch>();
        }

        public void SetName(string name)
        {
            var node = Document.ChildNodes[0];
            var innernode = Document.CreateElement("id");
            var attribute = Document.CreateAttribute("game");
            attribute.Value = name;
            innernode.Attributes.Append(attribute);
            node.AppendChild(innernode);
        }

        public Patch CreatePatch(string name)
        {
            Patches.Add(new Patch
            {
                Name = name,
                FolderPatches = new List<FolderPatch>(),
                MemoryPatches = new List<MemoryPatch>(),
                SavePatch = null
            });
            return Patches.Last();
        }

        public override string ToString()
        {
            Document.ChildNodes[0].AppendChild(options.ToElement(ref Document));
            Patches.ForEach(x => Document.ChildNodes[0].AppendChild(x.ToElement(ref Document)));
            var res = Document.Beautify();
            var lines = res.Split(new string[] { Environment.NewLine }, 0).ToList();
            lines.RemoveAt(0);
            res = string.Join(Environment.NewLine, lines);
            return res;
        }

        #region Sub Types
        #region Options
        public struct Options
        {
            public List<Section> Sections;

            internal XmlElement ToElement(ref XmlDocument xml)
            {
                var node = xml.CreateElement("options");
                foreach (var section in Sections)
                {
                    var s = xml.CreateElement("section");
                    s.SetAttribute("name", section.Name);
                    foreach (var option in section.Options)
                    {
                        var o = xml.CreateElement("option");
                        o.SetAttribute("name", option.Name);
                        foreach (var choice in option.Choices)
                        {
                            var c = xml.CreateElement("choice");
                            c.SetAttribute("name", choice.Name);
                            foreach (var patch in choice.Patches)
                            {
                                var p = xml.CreateElement("patch");
                                p.SetAttribute("id", patch);
                                c.AppendChild(p);
                            }
                            o.AppendChild(c);
                        }
                        s.AppendChild(o);
                    }
                    node.AppendChild(s);
                }
                return node;
            }

            public Section CreateSection(string name)
            {
                Sections.Add(new Section
                {
                    Options = new List<Option>(),
                    Name = name
                });
                return Sections.Last();
            }
        }

        public struct Section
        {
            public List<Option> Options;

            public string Name;

            public Option CreateOption(string name)
            {
                Options.Add(new Option
                {
                    Choices = new List<Choice>(),
                    Name = name
                });
                return Options.Last();
            }
        }

        public struct Option
        {
            public List<Choice> Choices;

            public string Name;

            public Choice CreateChoice(string name)
            {
                Choices.Add(new Choice
                {
                    Patches = new List<string>(),
                    Name = name
                });
                return Choices.Last();
            }
        }

        public struct Choice
        {
            public string Name;

            public List<string> Patches;
        }
        #endregion
        #region Patch
        public struct Patch
        {
            public string Name;

            public List<FolderPatch> FolderPatches;

            public List<MemoryPatch> MemoryPatches;

            public SavePatch? SavePatch;

            public FolderPatch CreateFolderPatch(string external, string disk = null, bool? recursive = null, bool? create = null)
            {
                FolderPatches.Add(new FolderPatch
                {
                    Recursive = recursive,
                    Disc = disk,
                    External = external,
                    Create = create
                });
                return FolderPatches.Last();
            }

            public MemoryPatch CreateMemoryPatch(uint offset, string value, string original, byte? region = null)
            {
                var m = new MemoryPatch
                {
                    Offset = offset,
                    Value = value,
                    Original = original,
                    Region = null
                };
                if (region != null)
                    m.Region = (Region)(byte)region;
                MemoryPatches.Add(m);
                return MemoryPatches.Last();
            }

            public SavePatch CreateSavePatch(string external, bool clone)
            {
                SavePatch = new SavePatch
                {
                    External = external,
                    Clone = clone
                };
                return (SavePatch)SavePatch;
            }

            internal XmlElement ToElement(ref XmlDocument xml)
            {
                var node = xml.CreateElement("patch");
                node.SetAttribute("id", Name);
                if (SavePatch != null)
                {
                    var save = xml.CreateElement("savegame");
                    save.SetAttribute("external", ((SavePatch)SavePatch).External);
                    save.SetAttribute("clone", ((SavePatch)SavePatch).Clone.ToString().ToLower());
                    node.AppendChild(save);
                }
                foreach (var folderpatch in FolderPatches)
                {
                    var f = xml.CreateElement("folder");
                    f.SetAttribute("external", folderpatch.External);
                    f.SetAttribute("disc", folderpatch.Disc ?? "/");
                    if (folderpatch.Recursive is null)
                    {
                        if (folderpatch.Create != null)
                        {
                            f.SetAttribute("create", ((bool)folderpatch.Create).ToString().ToLower());
                        }
                    } else
                    {
                        f.SetAttribute("recursive", folderpatch.Recursive.ToString().ToLower());
                        if (folderpatch.Create != null)
                        {
                            f.SetAttribute("create", ((bool)folderpatch.Create).ToString().ToLower());
                        }
                    }
                    node.AppendChild(f);
                }
                foreach (var memorypatch in MemoryPatches)
                {
                    var m = xml.CreateElement("memory");
                    m.SetAttribute("offset", $"0x{memorypatch.Offset:X}");
                    m.SetAttribute("value", memorypatch.Value);
                    m.SetAttribute("original", memorypatch.Original);
                    if (memorypatch.Region != null)
                        m.SetAttribute("target", ((Region)memorypatch.Region).ToString());
                    node.AppendChild(m);
                }
                return node;
            }
        }

        public struct FolderPatch
        {
            public bool? Recursive;

            public string Disc;

            public string External;

            public bool? Create;
        }

        public struct MemoryPatch
        {
            public uint Offset;

            public string Value;

            public string Original;

            public Region? Region;
        }

        public enum Region : byte
        {
            E = 69,
            J = 74,
            K = 75,
            P = 80,
            W = 87
        }

        public struct SavePatch
        {
            public string External;

            public bool Clone;
        }
        #endregion
        #endregion
    }
}
