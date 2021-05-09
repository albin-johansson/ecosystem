"""
This module is responsible for visualising food consumption.
"""

from pathlib import Path

import matplotlib.pyplot as plot

from logdata import *


def visualise_food_consumption(data: LogData, directory: Path):
  """
  Produces a plot of how the food availability changed over the course
  of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  figure, axes = plot.subplots()

  food_history = get_food_history(data)

  axes.plot(food_history.keys(), food_history.values(), label="Food", color="blue", **{"ls": "--"})

  axes.legend(loc="upper left")
  axes.set_xlim(0, data.duration_secs())
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Amount")
  axes.set_title("Food availability")

  plot.savefig(directory / Path("food_consumption.png"))
  plot.close()
