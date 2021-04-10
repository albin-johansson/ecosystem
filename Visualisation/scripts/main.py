import os
import sys
import getopt
from pathlib import Path

from logdata import LogData
from genome import visualise_genome_changes


def runGenome(data: LogData, directory):
  visualise_genome_changes(data, Path())
  print("runGenomeWasRun")


def main2():
  input_file: str = 'example.json'
  # directory: Path = input_file.with_suffix('')
  # if not directory.exists():
  # os.mkdir(directory)

  data = LogData(input_file)
  print(data.initial_rabbit_count())
  runGenome(data)
  print("temp main was run")


if __name__ == "__main__":
  main2()
