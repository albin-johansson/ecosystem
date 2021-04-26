import os
import sys
import getopt
from pathlib import Path

from logdata import LogData
from genome import visualise_genome_changes


# TODO: remove file before merge

def main2():
  input_file: str = 'example.json'
  directory: Path = input_file.with_suffix('')
  if not directory.exists():
    os.mkdir(directory)

  data = LogData(input_file)
  visualise_genome_changes(data, directory)
  # print("temp main is done")


if __name__ == "__main__":
  main2()
