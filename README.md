# Telegram logging extensions

[![Sponsor on GitHub](https://img.shields.io/badge/Sponsor_on_GitHub-ff7f00?logo=github&logoColor=white&style=for-the-badge)](https://github.com/sponsors/a-gubskiy)
[![Subscribe on X](https://img.shields.io/badge/Subscribe_on_X-000000?logo=x&logoColor=white&style=for-the-badge)](https://x.com/i/subscribe/andrew_gubskiy)

* X.Extensions.Logging.Telegram 
* X.Extensions.Serilog.Sinks.Telegram

Telegram logging providers for Serilog and standard .NET logging.

## What is Telegram?
What is Telegram? What do I do here?
Telegram is a messaging app with a focus on speed and security, it’s super-fast, simple and free. You can use Telegram on all your devices at the same time — your messages sync seamlessly across any number of your phones, tablets or computers. Telegram has over 500 million monthly active users and is one of the 10 most downloaded apps in the world.

## Why do you need to write logs to Telegram?
Because it is very comfortable - you can receive important messages directly to your smartphone or laptop.

## Prepare Telegram bot
For sending log messages into telegram channel or chat, you need to create telegram bot before. [Here](https://core.telegram.org/bots#3-how-do-i-create-a-bot) you can find how to do it.
After you create bot, add it to a channel with admin role and allow bot to post messages.

## Prepare Telegram channel
In the telegram, there are two types of channels: public and private. For public channel you can use channel name as *ChatId* in configuration. 

For private channel you can use [@JsonDumpBot](https://t.me/jsondumpbot) to get private channel id. Just forward any message from private channelto this bot. Additional information you can find [here](https://botostore.com/c/jsondumpbot/).

**Do not forget** to add your bot as admin with _write messages_ permission to channel.

## Logger implementations

### X.Extensions.Logging.Telegram
Read library documentation [here](./src/X.Extensions.Logging.Telegram/README.md).

### X.Extensions.Serilog.Sinks.Telegram
Read library documentation [here](./src/X.Extensions.Serilog.Sinks.Telegram/README.md).

# Examples

This repository includes several example projects that demonstrate how to use both libraries  in various scenarios. 
These examples can be helpful if you're starting or looking to use a specific feature.


# Contributing
Feel free to add any improvements you want via pull requests. All pull requests must be linked to an issue.

# License
This project is licensed under the MIT License.
