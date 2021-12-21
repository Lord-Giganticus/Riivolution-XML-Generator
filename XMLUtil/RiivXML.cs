using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.CompilerServices;

namespace XMLUtil
{
    public sealed class RiivXML
    {
        private readonly XmlDocument Document;

        public RiivOptions Options { get; internal set; }

        public List<Patch> Patches { get; internal set; }

        public RiivXML()
        {
            Document = new XmlDocument();
            var node = Document.CreateElement("wiidisc");
            var attribute = Document.CreateAttribute("version");
            attribute.Value = "1";
            node.Attributes.Append(attribute);
            Document.AppendChild(node);
            Options = new RiivOptions
            {
                Sections = new List<Section>()
            };
            Patches = new List<Patch>();
        }

        public void SetName(string name)
        {
            var node = Document.ChildNodes[0];
            var innernode = Document.CreateElement("id");
            innernode.SetAttribute("game", name);
            node.AppendChild(innernode);
        }

        public void SetNameAndRegions(string name)
        {
            var node = Document.ChildNodes[0];
            var game = Document.CreateElement("id");
            game.SetAttribute("game", name);
            foreach (var region in Extensions.GetEnumValues<Region>())
            {
                var innernode = Document.CreateElement("region");
                innernode.SetAttribute("type", region.ToString());
                game.AppendChild(innernode);
            }
            node.AppendChild(game);
        }

        public Patch CreatePatch(string name, string rootfile = null)
        {
            Patches.Add(new Patch
            {
                Name = name,
                FolderPatches = new List<FolderPatch>(),
                MemoryPatches = new List<MemoryPatch>(),
                RootFile = rootfile,
                SaveGamePatches = new List<SaveGamePatch>()
            });
            return Patches.Last();
        }
        
        public override string ToString()
        {
            var document = new XmlDocument();
            document.LoadXml(Document.OuterXml);
            document.ChildNodes[0].AppendChild(Options.ToElement(ref document));
            Patches.ForEach(x => document.ChildNodes[0].AppendChild(x.ToElement(ref document)));
            var res = document.Beautify();
            var lines = res.Split(new string[] { Environment.NewLine }, 0).ToList();
            lines.RemoveAt(0);
            res = string.Join(Environment.NewLine, lines);
            return res;
        }

        public unsafe byte[] ToBytes()
        {
            var size = Unsafe.SizeOf<RiivXML>();
            var bytes = new byte[size];
            var xml = this;
            Marshal.Copy((IntPtr)Unsafe.AsPointer(ref xml), bytes, 0, size);
            return bytes;
        }

        public unsafe static RiivXML FromBytes(byte[] src)
        {
            RiivXML res = new RiivXML();
            Unsafe.Copy(ref res, (void*)Marshal.UnsafeAddrOfPinnedArrayElement(src, 0));
            return res;
        }

        #region Sub Types
        #region Options
        public struct RiivOptions
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

            public string RootFile;

            public List<SaveGamePatch> SaveGamePatches;

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
                    Region = null,
                    ValueFile = null,
                };
                if (region != null)
                    m.Region = (Region)(byte)region;
                MemoryPatches.Add(m);
                return MemoryPatches.Last();
            }

            public MemoryPatch CreateMemoryPatch(uint offset, string valuefile, byte? region = null)
            {
                var m = new MemoryPatch
                {
                    Offset = offset,
                    ValueFile = valuefile,
                    Value = null,
                    Original = null,
                    Region = null
                };
                if (region != null)
                    m.Region = (Region)(byte)region;
                MemoryPatches.Add(m);
                return MemoryPatches.Last();
            }

            public SaveGamePatch CreateSaveGamePatch(string external, bool clone)
            {
                SaveGamePatches.Add(new SaveGamePatch
                {
                    External = external,
                    Clone = clone
                });
                return SaveGamePatches.Last();
            }

            internal XmlElement ToElement(ref XmlDocument xml)
            {
                var node = xml.CreateElement("patch");
                node.SetAttribute("id", Name);
                if (RootFile != null)
                    node.SetAttribute("root", RootFile);
                foreach (var savegame in SaveGamePatches)
                {
                    var s = xml.CreateElement("savegame");
                    s.SetAttribute("external", savegame.External);
                    s.SetAttribute("clone", savegame.Clone.ToString().ToLower());
                    node.AppendChild(s);
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
                    if (memorypatch.ValueFile is null)
                    {
                        m.SetAttribute("value", memorypatch.Value);
                        m.SetAttribute("original", memorypatch.Original);
                    } else
                    {
                        m.SetAttribute("valuefile", memorypatch.ValueFile);
                    }
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

            public string ValueFile;

            public Region? Region;
        }

        public struct SaveGamePatch
        {
            public string External;

            public bool Clone;
        }

        public enum Region : byte
        {
            E = 69,
            J = 74,
            K = 75,
            P = 80,
            W = 87
        }
        #endregion
        #endregion
    }
}
