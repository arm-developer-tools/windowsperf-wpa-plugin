# WPA-plugin
WPA-plugin is a plugin built for WPA that parses `json` output of `wperf` to visualize counting and telemetry events as timeline graphs.

## WPA
###  What is WPA
Windows Performance Analyzer (WPA) is a tool that creates graphs and data tables of Event Tracing for Windows (ETW) events
that are recorded by Windows Performance Recorder (WPR), Xperf, or an assessment that is run in the
Assessment Platform. WPA can open any event trace log (ETL) file for analysis.

### Installation
WPA is included in the Windows Assessment and Deployment Kit (Windows ADK) that can be downloaded [here](https://go.microsoft.com/fwlink/?linkid=2243390).

> The wperf WPA plugin requires a WPA version of `11.0.7.2` or higher.

Once downloaded, make sure that "Windows Performance Toolkit" checkbox is checked under "Select the features you want to install".

## Plugin

WPA-plugin is built on the [`microsoft-performance-toolkit-sdk`](https://github.com/microsoft/microsoft-performance-toolkit-sdk) and is shipped as a single
`.dll` file.

### Installation

There are 2 different methods to install the plugin:

- Moving the plugin dll to the WPA directory (defaults to `C:\\Program Files (x86)\Windows Kits\10\Windows Performance Toolkit`).
- Calling `wpa` from the command line and passing the plugin directory to the `-addsearchdir` flag (example : `wpa -addsearchdir "%USERPROFILE%\plugins"`).

> To verify that the plugin is loaded successfully, launch WPA then the plugin should appear under Help > About Windows Performance Analyzer.


### Usage
 - Start a `wperf` counting job (example: `wperf stat -t -i 0 -m imix,l1d_cache_miss_ratio,l1d_cache_mpki,l1d_tlb_miss_ratio,l1d_tlb_mpki -e inst_spec,vfp_spec,ld_spec,st_spec -c 1 --json`).
 - Save the output in a `.json` file.
 - Import the file in WPA (File > Open...)


 ## Project Structure

```bash
└───WPAPlugin
    ├───Constants   (Contains constants used throughout the plugin)
    ├───DataCookers (Contains the data cookers that converts events from their generic form to be consumable by the tables)
    ├───Events      (Contains the events parsed from the source parser as well as the events cooked by the data cookers )
    ├───Parsers     (Contains the json parsers based on Newtonsoft.Json)
    ├───Schemas     (Contains the json validation schemas)
    ├───Tables      (Contains the table definitions and configurations to be displayed in WPA)
    └───Utils       (Contains utility functions and classes)
```


## Contributing

To contribute to the project follow our [Contributing Guidelines](CONTRIBUTING.md).

## License

All code in this repository is licensed under the [BSD 3-Clause License](LICENSE).
