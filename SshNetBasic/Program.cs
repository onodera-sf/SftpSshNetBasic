using System.IO;
using Renci.SshNet;

namespace SshNetBasic
{
	class Program
	{
		static void Main(string[] args)
		{
			// 必要な情報を設定する
			var host = "<HostName>";
			var userName = "<UserName>";
			var passPhrase = "<PassPhrase>";
			var keyFilePath = @"<Private Key File Path>";
			var sendFilePath = @"<Send File Path>";
			var reseiveFilePath = @"<Save File Path>";

			// 認証メソッドを作成
			var authMethod = new PrivateKeyAuthenticationMethod(userName, new PrivateKeyFile(keyFilePath, passPhrase));

			// 接続情報を作成
			var connectionInfo = new ConnectionInfo(host, userName, authMethod);

			// SFTP クライアントを作成
			var client = new SftpClient(connectionInfo);
			
			// 接続。失敗した場合は例外が発生
			client.Connect();

			// ファイルのアップロード（上書き）
			using var sendStream = File.OpenRead(sendFilePath);
			client.UploadFile(sendStream, Path.GetFileName(sendFilePath), true);

			// ファイルのダウンロード（上書き）
			using var reseiveStream = File.OpenWrite(reseiveFilePath);
			client.DownloadFile(Path.GetFileName(sendFilePath), reseiveStream);

			// 切断
			client.Disconnect();
		}
	}
}
