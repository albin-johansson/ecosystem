import os
import sys
import getopt
from pathlib import Path

from logdata import LogData
from animalpopulation import visualise_animal_populations
from foodconsumption import visualise_food_consumption


# Runs all available visualisations on a simulation data file
def visualise(input_file: Path):
  directory: Path = input_file.with_suffix('')

  if not directory.exists():
    os.mkdir(directory)

  data = LogData(input_file)
  visualise_animal_populations(data, directory)
  visualise_food_consumption(data, directory)


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
