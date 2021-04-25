"""
This module is responsible for visualising changes in the animal populations.
"""

import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *


def tmp(data: LogData):  # , directory: Path):
  # TODO: make into method, call for each animals genes.
  values = []
  times = []
  for g in data.wolfHungerRate():
    values.append(g.value)
    times.append(g.time)

  plot.plot(times, values, 'ro')
  plot.show()


def int_to_gene_type(code):
  return {
    0: "HungerRate",
    1: "HungerThreshold",
    2: "ThirstRate",
    3: "ThirstThreshold",
    4: "Vision",
    5: "SpeedFactor",
    6: "SizeFactor",
    7: "DesirabilityScore",
    8: "GestationPeriod",
    9: "SexualMaturityTime"
  }[code]


def visualise_genome_changes(data: LogData):
  """
  Produces two plots of how the predator and prey populations changed over the course
  of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """
  """
  visualise_animal_populations_standard(data, directory)
  visualise_animal_populations_stackplot(data, directory)
  """
  tmp(data)
