# Unity 6 × AI ワークショップ スターター

高校生向け「3Dものづくり × AI」ワークショップで使用するUnity 6実習用プロジェクトです。

## この実習で行うこと

Visual Studio 2022でC#コードを編集し、Unity 6ではPlayして結果を確認します。ボールへ力を加え、ブロックを倒す物理演算シーンを完成させます。

## 使用環境

- Unity 6（基準バージョン：6000.2.11f1）
- Visual Studio 2022
- Git
- GitHub Copilot、またはブラウザ版Gemini

## 最初の準備

1. Visual Studio 2022を起動します。
2. 「リポジトリのクローン」を選びます。
3. このリポジトリのURLを入力します。
4. クローン先には、分かりやすいローカルフォルダを指定します。
5. Unity Hubで「追加」または「Add project from disk」を選び、クローンしたフォルダを開きます。
6. `Assets/Scenes/PhysicsBowling.unity`を開きます。
7. 初回だけUnityの`Assets > Open C# Project`を選び、Unity用のSolutionをVisual Studioで開きます。

公開リポジトリのクローンにはGitHubアカウントは不要です。

## 実習の進め方

1. UnityでPlayし、最初の状態を確認します。
2. Visual Studioで`Assets/Scripts/BallLauncher.cs`を開きます。
3. GitHub Copilot AgentまたはGeminiへ、ボールを発射するコードを依頼します。
4. `BallLauncher.cs`を編集して保存します。
5. Unityへ戻り、コンパイルが終わるまで待ちます。
6. Playし、ボールがブロックへ飛ぶことを確認します。
7. 力の数値を変え、動きの違いを試します。

## AIへ入力する内容

```text
Unity 6の3Dプロジェクトです。Assets/Scripts/BallLauncher.csを編集してください。
Sphereに取り付け、ゲーム開始時にRigidbodyへForceMode.Impulseを使って、
上方向に4、前方向に8の力を一度だけ加えてください。
力はInspectorから変更できるVector3型のlaunchForceにしてください。
[RequireComponent(typeof(Rigidbody))]を付け、クラス名はBallLauncherのままにしてください。
```

GitHub Copilotを利用できない場合は、この文章をGeminiへ入力し、生成されたC#コード全体を`BallLauncher.cs`へ貼り付けます。

## Visual Studio 2022でCopilot Agentを使う場合

1. Visual Studioへ自分のGitHubアカウントでサインインします。
2. 右上のGitHub CopilotアイコンからChatを開きます。
3. モードを`Agent`に変更します。
4. 上記の依頼文を入力します。
5. `BallLauncher.cs`の変更内容を確認してから採用します。
6. Unityへ戻ってPlayします。

Agentモードを利用できない場合は、Copilot Chatの回答またはGeminiの回答を自分で貼り付けます。

## 次回までの宿題

第2回までに、自分のGitHubアカウントを作成してください。アカウントのユーザー名、パスワード、認証コードはAIチャットへ入力しないでください。

## 困ったとき

- UnityのConsoleに赤いエラーが出た場合は、最初の赤いエラーを確認します。
- ファイル名とクラス名がどちらも`BallLauncher`になっているか確認します。
- Unityがコンパイル中の間はPlayを押さず、右下の処理表示が終わるまで待ちます。
- GitHubのパスワードや個人情報をAIへ入力しないでください。

