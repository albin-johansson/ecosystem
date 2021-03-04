"""
This module is responsible for visualising changes in the animal populations.
"""

import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *

rabbit_color = "#1f77b4"
deer_color = "#ff7f0e"
wolf_color = "#2ca02c"
bear_color = "#d62728"


def visualise_animal_populations_standard(data: LogData, directory: Path):
  """
  Produces an ordinary plot of how the animal populations changed over the course of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  rabbit_history = get_population_history(data, ["Rabbit"], data.initial_rabbit_count())
  deer_history = get_population_history(data, ["Deer"], data.initial_deer_count())
  wolf_history = get_population_history(data, ["Wolf"], data.initial_wolf_count())
  bear_history = get_population_history(data, ["Bear"], data.initial_bear_count())

  figure, axes = plot.subplots()

  axes.plot(rabbit_history.keys(), rabbit_history.values(), label="Rabbits", color=rabbit_color, **{"ls": "-."})
  axes.plot(deer_history.keys(), deer_history.values(), label="Deer", color=deer_color, **{"ls": "--"})
  axes.plot(wolf_history.keys(), wolf_history.values(), label="Wolves", color=wolf_color, **{"ls": "-."})
  axes.plot(bear_history.keys(), bear_history.values(), label="Bears", color=bear_color, **{"ls": "--"})

  axes.legend(loc="upper left")
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, max(data.initial_total_alive_count(), data.alive_count()) + 20)
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

  rabbit_count = data.initial_rabbit_count()
  deer_count = data.initial_deer_count()
  wolf_count = data.initial_wolf_count()
  bear_count = data.initial_bear_count()

  times: list[TimePoint] = []

  rabbit_history: list[Amount] = []
  deer_history: list[Amount] = []
  wolf_history: list[Amount] = []
  bear_history: list[Amount] = []

  times.append(0)
  rabbit_history.append(rabbit_count)
  deer_history.append(deer_count)
  wolf_history.append(wolf_count)
  bear_history.append(bear_count)

  for event in data.events():
    event_type: str = event["type"]
    tag: str = event["tag"]

    time: TimePoint = event["time"] / 1_000
    times.append(time)

    if event_type == "birth":
      if tag == "Rabbit":
        rabbit_count = rabbit_count + 1

      elif tag == "Deer":
        deer_count = deer_count + 1

      elif tag == "Wolf":
        wolf_count = wolf_count + 1

      elif tag == "Bear":
        bear_count = bear_count + 1

    elif event_type == "death":
      if tag == "Rabbit":
        rabbit_count = rabbit_count - 1

      elif tag == "Deer":
        deer_count = deer_count - 1

      elif tag == "Wolf":
        wolf_count = wolf_count - 1

      elif tag == "Bear":
        bear_count = bear_count - 1

    rabbit_history.append(rabbit_count)
    deer_history.append(deer_count)
    wolf_history.append(wolf_count)
    bear_history.append(bear_count)

  times.append(data.duration_secs())
  rabbit_history.append(rabbit_count)
  deer_history.append(deer_count)
  wolf_history.append(wolf_count)
  bear_history.append(bear_count)

  population_by_tag = {
    "Rabbits": rabbit_history,
    "Deer": deer_history,
    "Wolves": wolf_history,
    "Bears": bear_history
  }

  return times, population_by_tag


def visualise_animal_populations_stackplot(data: LogData, directory: Path):
  """
  Produces a stackplot of how the animals populations changed over the course of the simulation.

  :param data: the simulation data to read from.
  :param directory:
  :return: the directory to which the plot will be saved.
  """

  times, population_by_tag = create_stackplot_data(data)

  figure, axes = plot.subplots()
  colors = [rabbit_color, deer_color, wolf_color, bear_color]
  axes.stackplot(times, population_by_tag.values(), labels=population_by_tag.keys(), colors=colors)
  axes.legend(loc='lower left')
  axes.set_title("Population size changes")
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Population size")
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, max(data.initial_total_alive_count(), data.alive_count()) + 20)

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
