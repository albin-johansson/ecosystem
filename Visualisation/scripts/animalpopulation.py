import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *


def visualise_animal_populations(data: LogData, directory: Path):
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
  axes.plot(predator_history.keys(), predator_history.values(), label="Predator", color="red", **{"ls": "--"})

  axes.legend(loc="upper left")
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, data.initial_total_alive_count())
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Population size")

  plot.savefig(directory / Path("animal_populations.png"))
  plot.close()
