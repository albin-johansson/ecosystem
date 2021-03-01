"""
This module is responsible for visualising changes in the animal populations.
"""

import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *


def visualise_animal_populations_standard(data: LogData, directory: Path):
  """
  Produces a plot of how the predator and prey populations changed over the course
  of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  prey_history = get_population_history(data, "Prey", data.initial_prey_count())
  predator_history = get_population_history(data, "Predator", data.initial_predator_count())

  figure, axes = plot.subplots()

  axes.plot(prey_history.keys(), prey_history.values(), label="Prey", color="green", **{"ls": "-"})
  axes.plot(predator_history.keys(), predator_history.values(), label="Predators", color="red", **{"ls": "--"})

  axes.legend(loc="upper left")
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, data.initial_total_alive_count())
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Population size")
  axes.set_title("Population sizes")

  plot.savefig(directory / Path("animal_populations.png"))
  plot.close()


def create_stackplot_data(data: LogData) -> tuple[list[TimePoint], dict[str, list[Amount]]]:
  """
  Creates the data required to produce a stackplot of the animal populations.

  :param data: the wrapper around the data that will be read.
  :return: the time points and labelled animal populations as a tuple.
  """

  prey_count = data.initial_prey_count()
  predator_count = data.initial_predator_count()

  times: list[TimePoint] = []
  prey_history: list[Amount] = []
  predator_history: list[Amount] = []

  times.append(0)
  prey_history.append(prey_count)
  predator_history.append(predator_count)

  for death in data.deaths():
    time: TimePoint = death["time"] / 1_000
    times.append(time)

    if death["tag"] == "Predator":
      predator_count = predator_count - 1

    elif death["tag"] == "Prey":
      prey_count = prey_count - 1

    prey_history.append(prey_count)
    predator_history.append(predator_count)

  times.append(data.duration_secs())
  predator_history.append(predator_count)
  prey_history.append(prey_count)

  population_by_tag = {
    "Prey": prey_history,
    "Predators": predator_history
  }

  return times, population_by_tag


def visualise_animal_populations_stackplot(data: LogData, directory: Path):
  """
  Produces a stackplot of how the predator and prey populations changed over the course
  of the simulation.
  :param data: the simulation data to read from.
  :param directory:
  :return: the directory to which the plot will be saved.
  """

  times, population_by_tag = create_stackplot_data(data)

  figure, axes = plot.subplots()
  axes.stackplot(times, population_by_tag.values(),
                 labels=population_by_tag.keys())
  axes.legend(loc='upper left')
  axes.set_title("Population size changes")
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Population size")
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, data.initial_total_alive_count() + 20)

  plot.savefig(directory / Path("animal_populations_stackplot.png"))
  plot.close()


def visualise_animal_populations(data: LogData, directory: Path):
  """
  Produces two plots of how the predator and prey populations changed over the course
  of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """
  visualise_animal_populations_standard(data, directory)
  visualise_animal_populations_stackplot(data, directory)
