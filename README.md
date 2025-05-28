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
| Save Failed Run | `true` | Save log for the current run even it failed. |
| Save Abandoned Run | `false` | Save log for the current run even it is abandoned. |
| Save Profile Name | `true` | Save and show profile name when uploaded to LBoL Logs. |
| Save Profiles Together | `true` | Save the logs of different profiles in the same directory.<br />If set to `false`, they are saved under the corresponding index, i.e. `0`/`1`/`2`. |
| Auto Upload Log #0<br />Auto Upload Log #1<br />Auto Upload Log #2 | `false` | Auto upload the log of Profile #`i` to LBoL Logs.<br />If set to `false`, you can upload with description at the result screen.<br />Uploaded log will be deleted from local drive. |