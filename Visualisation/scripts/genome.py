"""
This module is responsible for visualising changes in the animal genomes.
"""

import os
from pathlib import Path

import matplotlib.pyplot as plot

from logdata import *


def make_averages(data: list[AverageGenomeEntry], directory: Path, animal: str, gene: str):
  figure, axes = plot.subplots()

  values: list[float] = []
  times: list[int] = []

  for entry in data:
    values.append(entry.value)
    times.append(int(entry.time / 1000))

  plot.plot(times, values, label=animal + "_" + gene, color="green")
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Average value")
  axes.set_title("Average value for the gene: " + gene)
  plot.savefig(directory / Path("average_" + animal + "_" + gene + ".png"))

  plot.close()


def make_boxplot(data: list[BoxGenomeEntry], directory: Path, animal: str, gene: str):
  values: list[list[float]] = []
  times: list[int] = []

  for entry in data:
    values.append(entry.values)
    times.append(int(entry.time / 1000))

  figure, axes = plot.subplots()

  axes.boxplot(values)
  axes.set_xticklabels(times)
  axes.set_xlabel("Time (seconds)")
  axes.set_ylabel("Values")
  axes.set_title("Boxplot of values for the gene: " + gene)

  plot.savefig(directory / Path("boxplot_" + animal + "_" + gene + ".png"))
  plot.close()


def plot_data(data: LogData, directory: Path, animal: str):
  if not os.path.exists(directory):
    os.makedirs(directory)

  make_averages(data.average_hunger_rate(animal), directory, animal, "hunger_rate")
  make_averages(data.average_hunger_threshold(animal), directory, animal, "hunger_threshold")
  make_averages(data.average_thirst_rate(animal), directory, animal, "thirst_rate")
  make_averages(data.average_thirst_threshold(animal), directory, animal, "thirst_threshold")
  make_averages(data.average_vision(animal), directory, animal, "vision")
  make_averages(data.average_speed(animal), directory, animal, "speed")
  make_averages(data.average_gestation_period(animal), directory, animal, "gestation_period")
  make_averages(data.average_sexual_maturity_time(animal), directory, animal, "sexual_maturity_time")

  make_boxplot(data.box_hunger_rate(animal), directory, animal, "hunger_rate")
  make_boxplot(data.box_hunger_threshold(animal), directory, animal, "hunger_threshold")
  make_boxplot(data.box_thirst_rate(animal), directory, animal, "thirst_rate")
  make_boxplot(data.box_thirst_threshold(animal), directory, animal, "thirst_threshold")
  make_boxplot(data.box_vision(animal), directory, animal, "vision")
  make_boxplot(data.box_speed(animal), directory, animal, "speed")
  make_boxplot(data.box_gestation_period(animal), directory, animal, "gestation_period")
  make_boxplot(data.box_sexual_maturity_time(animal), directory, animal, "sexual_maturity_time")


def pop_plot(data: list[BoxGenomeEntry], directory: Path, animal: str, gene: str):
  # TODO: tmp always speed
  # data = data.box_speed("rabbit")
  value_set = set[int]()
  for entry in data:
    for value in entry.values:
      value_set.add(value)
  # Populate list with list for each possible value
  pop_lists: [(float, list(int))] = []
  times: [int] = []
  temps: [int] = [0] * len(data)

  # TODO: make section into fewer loops.
  for i in range(0, len(data)):
    times.append(data[i].time)

  for value in value_set:
    pop_lists.append((value, temps.copy()))
  # </TODO>

  for i in range(0, len(data)):
    for entry in data[i].values:
      for pop in pop_lists:
        if pop[0] == entry:
          pop[1][i] = pop[1][i] + 1

  figure, axes = plot.subplots()
  # TODO: add time to the bottom
  colours: [(float, str)] = []
  for pop in pop_lists:
    p = axes.plot(pop[1], label="value: " + str(pop[0]))
    colours.append((pop[0], p[0].get_color()))
  # plot.show()
  axes.legend(loc=0)
  plot.savefig(directory / Path("gene_pop_" + animal + "_" + gene + ".png"))
  plot.close()


def visualise_genome_changes(data: LogData, directory: Path):
  """
  Produces two plots of how the predator and prey populations changed over the course
  of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """
  pop_plot(data.box_speed("rabbit"), directory, "rabbit", "speed")
  # plot_data(data, Path(os.path.join(directory, "rabbit")), "rabbit")
  # plot_data(data, Path(os.path.join(directory, "deer")), "deer")
  # plot_data(data, Path(os.path.join(directory, "wolf")), "wolf")
  # plot_data(data, Path(os.path.join(directory, "bear")), "bear")
