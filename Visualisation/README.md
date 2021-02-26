# Visualisation

When running the simulation, data is continuously collected and stored in-memory. Then, when the simulation ends, this data is dumped to a JSON file. This JSON file can be processed and 
visualised using the scripts found in this repository.

## Usage

In order to generate visualisations from simulation data, you can use the `visualise.py` script to generate all available plots from the data. The script takes a path to
the JSON file that contains the simulation data. It will create a directory with the same name as the input file, which contains all of the generated plots.

```bash
python visualise.py -i log_26-02-2021-222350.json
```

## Requirements

* [Python](https://www.python.org/), we use version 3.9.0.
* [Matplotlib](https://github.com/matplotlib/matplotlib), see below for how to install.
* Whilst not strictly required, an IDE such as JetBrains PyCharm is very useful.

## Installation

Use Pip to install matplotlib, with the following commands.

```bash
python -m pip install -U pip
python -m pip install -U matplotlib
```

We have tested our scripts using version 3.3.4, but they might work with other versions as well. You can check your local version of matplotlib using the following command.

```bash
pip show matplotlib
```
