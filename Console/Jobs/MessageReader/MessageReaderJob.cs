using Infrastructure.Repositories;
using Quartz;

namespace Console.Jobs.MessageReader
{
    [DisallowConcurrentExecution]
    public class MessageReaderJob : IJob
    {
        private readonly MessageRepository _messageRepository;

        public MessageReaderJob() { _messageRepository = new MessageRepository(); }

        private void WriteInFile(string message)
        {
            var projectRootDirectory = String.Join(
                Path.DirectorySeparatorChar,
                Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).Take(6).ToList());

            var path = Path.Combine(projectRootDirectory, "ReadMessages");
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, "ReadMessages.txt");
            using FileStream stream = new FileStream(path, FileMode.Append);
            using TextWriter textWriter = new StreamWriter(stream);
            textWriter.WriteLine(message);
        }

        public Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine("Started the job");
            var updatedMessages = _messageRepository.RunChangeMessageStatusSP(CancellationToken.None);

            string timeLogString = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");

            foreach(var updatedMessage in updatedMessages)
                WriteInFile($"Read a message at {timeLogString} => " + updatedMessage.ToString());

            System.Console.WriteLine("Completed the job");

            return Task.CompletedTask;
        }
    }
}
