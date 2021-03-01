"""
This module provides visualisations for the causes of animal deaths.
"""

import matplotlib.pyplot as plot
import numpy

from pathlib import Path
from logdata import *
from enum import Enum


def cause_of_death_index_to_string(index: int) -> str:
  """
  Converts a cause of death index to a human-readable string.

  :param index: the cause of death index that will be converted.
  :return: a human-readable string.
  :raises ValueError: if the supplied index isn't recognized.
  """

  if index == 0:
    return "Starvation"

  elif index == 1:
    return "Dehydration"

  elif index == 2:
    return "Eaten"

  else:
    raise ValueError("Did not recognize CauseOfDeath index!")


class CauseOfDeath(Enum):
  """
  A simple enum that mirrors the *Ecosystem.Logging.CauseOfDeath* enum.
  """

  STARVATION = 0
  DEHYDRATION = 1
  EATEN = 2

  def __str__(self) -> str:
    return cause_of_death_index_to_string(self.value)


def attach_text_labels(rects, axes):
  """
  Attaches a text label above each bar which displays the value (height) of the bar.

  :param rects: the bars that will have text labels added to them.
  :param axes: used to annotate the bars.
  """

  for rect in rects:
    height = rect.get_height()
    label = "{}".format(height)
    axes.annotate(label,
                  xy=(rect.get_x() + rect.get_width() / 2, height),
                  xytext=(0, 3),
                  textcoords="offset points",
                  ha='center', va='bottom')


def visualise_cause_of_death(data: LogData, directory: Path):
  """
  Produces a plot of how the food availability changed over the course
  of the simulation, along with the amount of food consumers.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  prey_stats: list[int] = [0 for _ in CauseOfDeath]
  predator_stats: list[int] = [0 for _ in CauseOfDeath]

  for death in data.deaths():
    tag: str = death["tag"]
    cause: int = death["cause"]

    if tag == "Prey":
      previous_value = prey_stats[cause]
      prey_stats[cause] = previous_value + 1

    elif tag == "Predator":
      previous_value = predator_stats[cause]
      predator_stats[cause] = previous_value + 1

  figure, axes = plot.subplots()

  labels = [str(e) for e in CauseOfDeath]
  x = numpy.arange(len(labels))

  width = 0.35
  rects_1 = axes.bar(x - width / 2, predator_stats, width, label="Predators")
  rects_2 = axes.bar(x + width / 2, prey_stats, width, label="Prey")

  axes.set_title("Deaths arranged by cause and animal type")
  axes.set_ylabel("Amount")
  axes.set_xticks(x)
  axes.set_xticklabels(labels)
  axes.legend()

  attach_text_labels(rects_1, axes)
  attach_text_labels(rects_2, axes)

  figure.tight_layout()
  figure.savefig(directory / Path("cause_of_death.png"))
  plot.close()
