"""
This is the core module for the data visualisation of the simulation data. It provides the
visualise-function which will run all available visualisations on a supplied log file. This module
is meant to be used from the command line using "python visualise.py -i my_log_file.json", which will
create a directory called "my_log_file" with all of the generated plots in it.
"""

import os
import sys
import getopt
from pathlib import Path

from logdata import LogData
from animalpopulation import visualise_animal_populations
from foodconsumption import visualise_food_consumption
from causeofdeath import visualise_cause_of_death
from genome import visualise_genome_changes


# Runs all available visualisations on a simulation data file
def visualise(input_file: Path):
  directory: Path = input_file.with_suffix('')

  if not directory.exists():
    os.mkdir(directory)

  data = LogData(input_file)
  #visualise_animal_populations(data, directory)
  #visualise_food_consumption(data, directory)
  #visualise_cause_of_death(data, directory)
  visualise_genome_changes(data,directory)


def main(argv):
  input_file: str = ''

  try:
    options, args = getopt.getopt(argv, "i:", ["input="])

    for option, argument in options:
      if option in ("-i", "--input"):
        input_file = argument

      else:
        print('Did not recognize input! Syntax: visualise.py -i <input_file>')
        sys.exit(2)

    visualise(Path(input_file))
  except getopt.GetoptError:
    print('Did not recognize input! Syntax: visualise.py -i <input_file>')
    sys.exit(2)


if __name__ == "__main__":
  main(sys.argv[1:])
