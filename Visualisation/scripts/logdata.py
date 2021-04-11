"""
This module provides utilities related to handling the simulation log data.
"""

import json

TimePoint = int
Amount = int


class LogData:
  """
  A wrapper around simulation data obtained from a JSON file.
  """

  data = {}

  def __init__(self, file_path) -> None:
    file = open(file_path)
    self.data = json.load(file)
    file.close()

  def duration_ms(self) -> int:
    return self.data["duration"]

  def duration_secs(self) -> int:
    return self.data["duration"] / 1_000

  def alive_count(self) -> int:
    return self.data["aliveCount"]

  def food_count(self) -> int:
    return self.data["foodCount"]

  def initial_food_count(self) -> int:
    return self.data["initialFoodCount"]

  def initial_total_alive_count(self) -> int:
    return self.data["initialAliveCount"]

  def initial_prey_count(self) -> int:
    return self.data["initialAlivePreyCount"]

  def initial_predator_count(self) -> int:
    return self.data["initialAlivePredatorCount"]

  def initial_rabbit_count(self) -> int:
    return self.data["initialAliveRabbitsCount"]

  def initial_deer_count(self) -> int:
    return self.data["initialAliveDeerCount"]

  def initial_wolf_count(self) -> int:
    return self.data["initialAliveWolvesCount"]

  def initial_bear_count(self) -> int:
    return self.data["initialAliveBearsCount"]

  def events(self):
    return self.data["events"]

  def death_info(self, index: int):
    return self.data["deaths"][index]

  def mating_info(self, index: int):
    return self.data["matings"][index]

  def genome_info(self):
    return self.data["genome"]

  def __getitem__(self, item):  # operator[]
    return self.data[item]


def get_population_history(data: LogData, tags: list[str], initial_count: Amount) -> dict[TimePoint, Amount]:
  """
  Returns the population history for a class of animal, e.g. rabbits or wolves.

  :param data: the data wrapper to read from.
  :param tags: a list of tags associated with the animals to obtain the history of.
  :param initial_count: the initial amount of animals.
  :return: a dictionary that maps time points (in seconds) to the associated population size.
  """

  history: dict[TimePoint, Amount] = {0: initial_count}
  count: Amount = initial_count

  for event in data.events():
    time: TimePoint = event["time"] / 1_000
    event_type: str = event["type"]
    event_tag: str = event["tag"]

    if event_tag in tags:
      if event_type == "death":
        count = count - 1

      elif event_type == "birth":
        count = count + 1

      history[time] = count

  history[data.duration_secs()] = count

  return history


def get_food_history(data: LogData) -> dict[TimePoint, Amount]:
  """
  Returns the food availability history.

  :param data: the data wrapper to read from.
  :return: a dictionary that maps time points (in seconds) to the associated food amount.
  """

  initial_food_count: int = data.initial_food_count()

  food_history: dict[TimePoint, Amount] = {0: initial_food_count}
  food_count: Amount = initial_food_count

  for event in data.events():
    time: TimePoint = event["time"] / 1_000
    event_type: str = event["type"]

    if event_type == "consumption":
      food_count = food_count - 1

    food_history[time] = food_count

  food_history[data.duration_secs()] = food_count

  return food_history


def is_predator(tag: str) -> bool:
  return tag == "Wolf" or tag == "Bear"


def is_prey(tag: str) -> bool:
  return tag == "Rabbit" or tag == "Deer"
