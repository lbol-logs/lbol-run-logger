# Description

An advanced logging mod for players to share runs on [LBoL Logs](https://lbol-logs.github.io/)

About: https://lbol-logs.github.io/about/

Changelog: https://github.com/lbol-logs/lbol-run-logger/commits/main/

# Configs

```
BepInEx\config\ev.lbol.utils.runLogger.cfg
```

| Config | Default | Description |
| --- | --- | --- |
| Auto Upload Log | `false` | Auto upload the log to LBoL Logs.<br />If set to `false`, an upload button is shown in the result screen (WIP).<br />Uploaded log will be deleted from local drive. |
| Save Profile Name | `true` | Save and show profile name when uploaded to LBoL Logs. |
| Save Failed Run | `true` | Save log for the current run even it failed. |
| Save Profiles Together | `true` | Save the logs of different profiles in the same directory.<br />If set to `false`, they are saved under the corresponding index, i.e. `0`/`1`/`2`. |