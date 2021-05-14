"""
This module provides visualisations for the causes of animal deaths.
"""

from enum import Enum
from pathlib import Path

import matplotlib.pyplot as plot
import numpy
from cycler import cycler

from logdata import *


class CauseOfDeath(Enum):
  """
  A simple enum that mirrors the *Ecosystem.Logging.CauseOfDeath* enum.
  """

  STARVATION = 0
  DEHYDRATION = 1
  EATEN = 2

  def __str__(self) -> str:
    return cause_of_death_index_to_string(self.value)


def cause_of_death_index_to_string(index: int) -> str:
  """
  Converts a cause of death index to a human-readable string.

  :param index: the cause of death index that will be converted.
  :return: a human-readable string.
  :raises ValueError: if the supplied index isn't recognized.
  """

  if index == CauseOfDeath.STARVATION.value:
    return "Starvation"

  elif index == CauseOfDeath.DEHYDRATION.value:
    return "Dehydration"

  elif index == CauseOfDeath.EATEN.value:
    return "Eaten"

  else:
    raise ValueError("Did not recognize CauseOfDeath index!")


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


def create_grouped_bar_chart(stats: dict[str, list[int]]):
  """
  Creates and returns a grouped bar chart with the death causes. The lists are expected
  to feature an entry for each cause of death, where the value corresponds to how many specimens
  died of that cause.

  :param stats: the stats associated with predators, maps labels to the corresponding stats.
  :return: the created figure.
  """

  figure, axes = plot.subplots()

  labels = [str(e) for e in CauseOfDeath]
  x = numpy.arange(len(labels))

  bar_width = 0.15
  max_value = 0

  b_c = (cycler('hatch', ['///', '--', '...', 'xxx', '\\\\']) * cycler('zorder', [10]) * cycler('color', 'w'))
  styles = b_c()
  rects = []
  i = 0
  for label, values in stats.items():
    max_value = max(max_value, max(values))
    rects.append(axes.bar(x + (i * bar_width), values, bar_width, label=label, **next(styles)))
    i = i + 1

  axes.set_title("Deaths arranged by cause and animal type")
  axes.set_ylabel("Amount")
  axes.set_xticks(x)
  axes.set_xticklabels(labels)
  axes.legend()

  for rect in rects:
    attach_text_labels(rect, axes)

  figure.tight_layout()
  return figure


def visualise_cause_of_death(data: LogData, directory: Path):
  """
  Produces a grouped bar chart of the different causes of deaths,
  arranged by the animal types.

  :param data: the simulation data to read from.
  :param directory: the directory to which the plot will be saved.
  """

  rabbit_stats: list[int] = [0 for _ in CauseOfDeath]
  deer_stats: list[int] = [0 for _ in CauseOfDeath]
  wolf_stats: list[int] = [0 for _ in CauseOfDeath]
  bear_stats: list[int] = [0 for _ in CauseOfDeath]

  for event in data.events():
    event_type: str = event["type"]

    if event_type == "death":
      tag: str = event["tag"]

      info = data.death_info(event["deathIndex"])
      cause: int = info["cause"]

      if tag == "Rabbit":
        rabbit_stats[cause] = rabbit_stats[cause] + 1

      elif tag == "Deer":
        deer_stats[cause] = deer_stats[cause] + 1

      elif tag == "Wolf":
        wolf_stats[cause] = wolf_stats[cause] + 1

      elif tag == "Bear":
        bear_stats[cause] = bear_stats[cause] + 1

  figure = create_grouped_bar_chart({"Rabbits": rabbit_stats,
                                     "Deer": deer_stats,
                                     "Wolves": wolf_stats,
                                     "Bears": bear_stats})
  figure.savefig(directory / Path("cause_of_death.png"))
  plot.close()
