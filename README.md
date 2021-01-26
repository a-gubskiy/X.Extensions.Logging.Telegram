# X.Extensions.Logging.Telegram
Telegram logging provider


## Prepare Telegram bot
For sending log messages into telegram channel or chat you need create telegram bot before. [Here](https://core.telegram.org/bots#3-how-do-i-create-a-bot) you can find how to do it.
After you created bot add it to channel  with  admin role and allow bot to post messages.

# Prepare Channel
In telegram there are two types of channels: public and private. For public channel you can use channel name as *ChatId* in configuration. 

For private channel you can use [@JsonDumpBot](https://t.me/jsondumpbot) to get private channel id. Just forward any message from private channelto this bot. Additional information you can find [here](https://botostore.com/c/jsondumpbot/).

## Configure Telegram Logging provider
