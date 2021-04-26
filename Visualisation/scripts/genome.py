"""
This module is responsible for visualising changes in the animal genomes.
"""
import os
import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *


def averages(list: [], directory: Path, animal: str, gene: str):
  figure, axes = plot.subplots()
  values = []
  times = []
  for g in list:
    values.append(g.value)
    times.append(g.time / 1000)

  plot.plot(times, values, label=animal + "_" + gene, color="green")
  axes.set_xlabel("Time (sec)")
  axes.set_ylabel("Average value")
  axes.set_title("Average gene value: " + gene)
  plot.savefig(directory / Path("Average_" + animal + "_" + gene + ".png"))
  plot.close()

  plot.show()


def boxplots(list: [], directory: Path, animal: str, gene: str):
  values = []
  times = []
  for g in list:
    values.append(g.value)
    times.append(g.time / 1000)

  figure, axes = plot.subplots()
  axes.boxplot(values)
  axes.set_xticklabels(times)
  axes.set_xlabel("Time (sec)")
  axes.set_ylabel("Box plot")
  axes.set_title("Boxplot gene value: " + gene)
  plot.savefig(directory / Path("Boxplot_" + animal + "_" + gene + ".png"))
  plot.close()


def visualise_genome_changes(data: LogData, directory: Path):
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
  rabbits = os.path.join(directory, r'rabbit_folder')
  if not os.path.exists(rabbits):
    os.makedirs(rabbits)
  wolfs = os.path.join(directory, r'wolf_folder')
  if not os.path.exists(wolfs):
    os.makedirs(wolfs)
  deers = os.path.join(directory, r'deer_folder')
  if not os.path.exists(deers):
    os.makedirs(deers)
  bears = os.path.join(directory, r'bear_folder')
  if not os.path.exists(bears):
    os.makedirs(bears)

  # rabbit averages:
  averages(data.rabbit_hunger_rate(), rabbits, "rabbit", "hunger_rate")
  averages(data.rabbit_hunger_threshold(), rabbits, "rabbit", "hunger_threshold")
  averages(data.rabbit_thirst_rate(), rabbits, "rabbit", "thirst_rate")
  averages(data.rabbit_thirst_threshold(), rabbits, "rabbit", "thirst_threshold")
  averages(data.rabbit_vision(), rabbits, "rabbit", "vision")
  averages(data.rabbit_speed(), rabbits, "rabbit", "speed")
  averages(data.rabbit_size_factor(), rabbits, "rabbit", "size_factor")
  averages(data.rabbit_desirability_score(), rabbits, "rabbit", "desirability_score")
  averages(data.rabbit_gestation_period(), rabbits, "rabbit", "gestation_period")
  averages(data.rabbit_sexual_maturity_time(), rabbits, "rabbit", "sexual_maturity_time")

  # deer averages:
  averages(data.deer_hunger_rate(), deers, "deer", "hunger_rate")
  averages(data.deer_hunger_threshold(), deers, "deer", "hunger_threshold")
  averages(data.deer_thirst_rate(), deers, "deer", "thirst_rate")
  averages(data.deer_thirst_threshold(), deers, "deer", "thirst_threshold")
  averages(data.deer_vision(), deers, "deer", "vision")
  averages(data.deer_speed(), deers, "deer", "speed")
  averages(data.deer_size_factor(), deers, "deer", "size_factor")
  averages(data.deer_desirability_score(), deers, "deer", "desirability_score")
  averages(data.deer_gestation_period(), deers, "deer", "gestation_period")
  averages(data.deer_sexual_maturity_time(), deers, "deer", "sexual_maturity_time")

  # wolf averages:
  averages(data.wolf_hunger_rate(), wolfs, "wolf", "hunger_rate")
  averages(data.wolf_hunger_threshold(), wolfs, "wolf", "hunger_threshold")
  averages(data.wolf_thirst_rate(), wolfs, "wolf", "thirst_rate")
  averages(data.wolf_thirst_threshold(), wolfs, "wolf", "thirst_threshold")
  averages(data.wolf_vision(), wolfs, "wolf", "vision")
  averages(data.wolf_speed(), wolfs, "wolf", "speed")
  averages(data.wolf_size_factor(), wolfs, "wolf", "size_factor")
  averages(data.wolf_desirability_score(), wolfs, "wolf", "desirability_score")
  averages(data.wolf_gestation_period(), wolfs, "wolf", "gestation_period")
  averages(data.wolf_sexual_maturity_time(), wolfs, "wolf", "sexual_maturity_time")

  # bear averages:
  averages(data.bear_hunger_rate(), bears, "bear", "hunger_rate")
  averages(data.bear_hunger_threshold(), bears, "bear", "hunger_threshold")
  averages(data.bear_thirst_rate(), bears, "bear", "thirst_rate")
  averages(data.bear_thirst_threshold(), bears, "bear", "thirst_threshold")
  averages(data.bear_vision(), bears, "bear", "vision")
  averages(data.bear_speed(), bears, "bear", "speed")
  averages(data.bear_size_factor(), bears, "bear", "size_factor")
  averages(data.bear_desirability_score(), bears, "bear", "desirability_score")
  averages(data.bear_gestation_period(), bears, "bear", "gestation_period")
  averages(data.bear_sexual_maturity_time(), bears, "bear", "sexual_maturity_time")

  # rabbit boxplots:
  boxplots(data.rabbit_hunger_rate_box(), rabbits, "rabbit", "hunger_rate")
  boxplots(data.rabbit_hunger_threshold_box(), rabbits, "rabbit", "hunger_threshold")
  boxplots(data.rabbit_thirst_rate_box(), rabbits, "rabbit", "thirst_rate")
  boxplots(data.rabbit_thirst_threshold_box(), rabbits, "rabbit", "thirst_threshold")
  boxplots(data.rabbit_vision_box(), rabbits, "rabbit", "vision")
  boxplots(data.rabbit_speed_box(), rabbits, "rabbit", "speed")
  boxplots(data.rabbit_size_factor_box(), rabbits, "rabbit", "size_factor")
  boxplots(data.rabbit_desirability_score_box(), rabbits, "rabbit", "desirability_score")
  boxplots(data.rabbit_gestation_period_box(), rabbits, "rabbit", "gestation_period")
  boxplots(data.rabbit_sexual_maturity_time_box(), rabbits, "rabbit", "sexual_maturity_time")

  # deer boxplots:
  boxplots(data.deer_hunger_rate_box(), deers, "deer", "hunger_rate")
  boxplots(data.deer_hunger_threshold_box(), deers, "deer", "hunger_threshold")
  boxplots(data.deer_thirst_rate_box(), deers, "deer", "thirst_rate")
  boxplots(data.deer_thirst_threshold_box(), deers, "deer", "thirst_threshold")
  boxplots(data.deer_vision_box(), deers, "deer", "vision")
  boxplots(data.deer_speed_box(), deers, "deer", "speed")
  boxplots(data.deer_size_factor_box(), deers, "deer", "size_factor")
  boxplots(data.deer_desirability_score_box(), deers, "deer", "desirability_score")
  boxplots(data.deer_gestation_period_box(), deers, "deer", "gestation_period")
  boxplots(data.deer_sexual_maturity_time_box(), deers, "deer", "sexual_maturity_time")

  # wolf boxplots:
  boxplots(data.wolf_hunger_rate_box(), wolfs, "wolf", "hunger_rate")
  boxplots(data.wolf_hunger_threshold_box(), wolfs, "wolf", "hunger_threshold")
  boxplots(data.wolf_thirst_rate_box(), wolfs, "wolf", "thirst_rate")
  boxplots(data.wolf_thirst_threshold_box(), wolfs, "wolf", "thirst_threshold")
  boxplots(data.wolf_vision_box(), wolfs, "wolf", "vision")
  boxplots(data.wolf_speed_box(), wolfs, "wolf", "speed")
  boxplots(data.wolf_size_factor_box(), wolfs, "wolf", "size_factor")
  boxplots(data.wolf_desirability_score_box(), wolfs, "wolf", "desirability_score")
  boxplots(data.wolf_gestation_period_box(), wolfs, "wolf", "gestation_period")
  boxplots(data.wolf_sexual_maturity_time_box(), wolfs, "wolf", "sexual_maturity_time")

  # bear boxplots:
  boxplots(data.bear_hunger_rate_box(), bears, "bear", "hunger_rate")
  boxplots(data.bear_hunger_threshold_box(), bears, "bear", "hunger_threshold")
  boxplots(data.bear_thirst_rate_box(), bears, "bear", "thirst_rate")
  boxplots(data.bear_thirst_threshold_box(), bears, "bear", "thirst_threshold")
  boxplots(data.bear_vision_box(), bears, "bear", "vision")
  boxplots(data.bear_speed_box(), bears, "bear", "speed")
  boxplots(data.bear_size_factor_box(), bears, "bear", "size_factor")
  boxplots(data.bear_desirability_score_box(), bears, "bear", "desirability_score")
  boxplots(data.bear_gestation_period_box(), bears, "bear", "gestation_period")
  boxplots(data.bear_sexual_maturity_time_box(), bears, "bear", "sexual_maturity_time")
