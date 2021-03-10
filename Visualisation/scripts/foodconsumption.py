"""
This module is responsible for visualising food consumption.
"""

import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *


def visualise_food_consumption(data: LogData, directory: Path):
  """
  Produces a plot of how the food availability changed over the course
  of the simulation, along with the amount of food consumers.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  figure, axes = plot.subplots()

  consumer_history = get_population_history(data, ["Rabbit", "Deer"], data.initial_prey_count())
  food_history = get_food_history(data)

  axes.plot(consumer_history.keys(), consumer_history.values(), label="Consumers", color="green", **{"ls": "-"})
  axes.plot(food_history.keys(), food_history.values(), label="Food", color="blue", **{"ls": "--"})

  axes.legend(loc="upper left")
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, max(max(food_history.values()), max(consumer_history.values())) + 20)
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Amount")
  axes.set_title("Food availability and consumer population")

  plot.savefig(directory / Path("food_consumption.png"))
  plot.close()
