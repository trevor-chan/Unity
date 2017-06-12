using System;
using System.Collections.Generic;
using System.Threading;

namespace GitHub.Unity
{
    class GitConfigSetTask : ProcessTask<string>
    {
        private readonly string value;
        private readonly string arguments;
        private string label;

        public GitConfigSetTask(string key, string value, GitConfigSource configSource,
            CancellationToken token, IOutputProcessor<string> processor = null)
            : base(token, processor ?? new SimpleOutputProcessor())
        {
            this.value = value;
            var source = "";
            source +=
                configSource == GitConfigSource.NonSpecified ? "" :
                    configSource == GitConfigSource.Local ? "--replace-all --local" :
                        configSource == GitConfigSource.User ? "--replace-all --global" :
                            "--replace-all --system";
            arguments = String.Format("config {0} {1} \"{2}\"", source, key, value);
            label = String.Format("config {0} {1} \"{2}\"", source, key, new String('*', value.Length));
        }

        public override string Name { get { return label;} }
        public override string ProcessArguments { get { return arguments; } }
        public override TaskAffinity Affinity { get { return TaskAffinity.Exclusive; } }
    }
}