"""
This module is responsible for visualising changes in the animal genomes.
"""

import os
import matplotlib.pyplot as plot
from pathlib import Path
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


def visualise_genome_changes(data: LogData, directory: Path):
  """
  Produces two plots of how the predator and prey populations changed over the course
  of the simulation.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  plot_data(data, Path(os.path.join(directory, "rabbit")), "rabbit")
  plot_data(data, Path(os.path.join(directory, "deer")), "deer")
  plot_data(data, Path(os.path.join(directory, "wolf")), "wolf")
  plot_data(data, Path(os.path.join(directory, "bear")), "bear")
