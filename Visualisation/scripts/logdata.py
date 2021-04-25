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
    return self.data["genomes"]

  def __getitem__(self, item):  # operator[]
    return self.data[item]

  def averageGenomes(self, animal: str, gene: str):
    # animal = "rabbitAverageGenomes"
    # gene = "HungerRate"
    list = self.data[animal][gene]
    list2 = []
    for v in list:
      list2.append(AverageGenomeEntry(v["entryTime"], v["value"]))
    return list2

  # Rabbit gets
  def rabbitHungerRate(self):
    return self.averageGenomes("rabbitAverageGenomes", "HungerRate")

  def rabbitHungerThreshold(self):
    return self.averageGenomes("rabbitAverageGenomes", "HungerThreshold")

  def rabbitThirstRate(self):
    return self.averageGenomes("rabbitAverageGenomes", "ThirstRate")

  def rabbitThirstThreshold(self):
    return self.averageGenomes("rabbitAverageGenomes", "ThirstThreshold")

  def rabbitVision(self):
    return self.averageGenomes("rabbitAverageGenomes", "Vision")

  def rabbitSpeed(self):
    return self.averageGenomes("rabbitAverageGenomes", "Speed")

  def rabbitSizeFactor(self):
    return self.averageGenomes("rabbitAverageGenomes", "SizeFactor")

  def rabbitDesirabilityScore(self):
    return self.averageGenomes("rabbitAverageGenomes", "DesirabilityScore")

  def rabbitGestationPeriod(self):
    return self.averageGenomes("rabbitAverageGenomes", "GestationPeriod")

  def rabbitSexualMaturityTime(self):
    return self.averageGenomes("rabbitAverageGenomes", "SexualMaturityTime")

  # Wolf gets
  def wolfHungerRate(self):
    return self.averageGenomes("wolfAverageGenomes", "HungerRate")

  def wolfHungerThreshold(self):
    return self.averageGenomes("wolfAverageGenomes", "HungerThreshold")

  def wolfThirstRate(self):
    return self.averageGenomes("wolfAverageGenomes", "ThirstRate")

  def wolfThirstThreshold(self):
    return self.averageGenomes("wolfAverageGenomes", "ThirstThreshold")

  def wolfVision(self):
    return self.averageGenomes("wolfAverageGenomes", "Vision")

  def wolfSpeed(self):
    return self.averageGenomes("wolfAverageGenomes", "Speed")

  def wolfSizeFactor(self):
    return self.averageGenomes("wolfAverageGenomes", "SizeFactor")

  def wolfDesirabilityScore(self):
    return self.averageGenomes("wolfAverageGenomes", "DesirabilityScore")

  def wolfGestationPeriod(self):
    return self.averageGenomes("wolfAverageGenomes", "GestationPeriod")

  def wolfSexualMaturityTime(self):
    return self.averageGenomes("wolfAverageGenomes", "SexualMaturityTime")

  # Deer gets
  def deerHungerRate(self):
    return self.averageGenomes("deerAverageGenomes", "HungerRate")

  def deerHungerThreshold(self):
    return self.averageGenomes("deerAverageGenomes", "HungerThreshold")

  def deerThirstRate(self):
    return self.averageGenomes("deerAverageGenomes", "ThirstRate")

  def deerThirstThreshold(self):
    return self.averageGenomes("deerAverageGenomes", "ThirstThreshold")

  def deerVision(self):
    return self.averageGenomes("deerAverageGenomes", "Vision")

  def deerSpeed(self):
    return self.averageGenomes("deerAverageGenomes", "Speed")

  def deerSizeFactor(self):
    return self.averageGenomes("deerAverageGenomes", "SizeFactor")

  def deerDesirabilityScore(self):
    return self.averageGenomes("deerAverageGenomes", "DesirabilityScore")

  def deerGestationPeriod(self):
    return self.averageGenomes("deerAverageGenomes", "GestationPeriod")

  def deerSexualMaturityTime(self):
    return self.averageGenomes("deerAverageGenomes", "SexualMaturityTime")

  # Bear gets
  def bearHungerRate(self):
    return self.averageGenomes("bearAverageGenomes", "HungerRate")

  def bearHungerThreshold(self):
    return self.averageGenomes("bearAverageGenomes", "HungerThreshold")

  def bearThirstRate(self):
    return self.averageGenomes("bearAverageGenomes", "ThirstRate")

  def bearThirstThreshold(self):
    return self.averageGenomes("bearAverageGenomes", "ThirstThreshold")

  def bearVision(self):
    return self.averageGenomes("bearAverageGenomes", "Vision")

  def bearSpeed(self):
    return self.averageGenomes("bearAverageGenomes", "Speed")

  def bearSizeFactor(self):
    return self.averageGenomes("bearAverageGenomes", "SizeFactor")

  def bearDesirabilityScore(self):
    return self.averageGenomes("bearAverageGenomes", "DesirabilityScore")

  def bearGestationPeriod(self):
    return self.averageGenomes("bearAverageGenomes", "GestationPeriod")

  def bearSexualMaturityTime(self):
    return self.averageGenomes("bearAverageGenomes", "SexualMaturityTime")


class AverageGenomeEntry:
  time = 0
  value = 0

  def __init__(self, time: int, value: int):
    self.time = time
    self.value = value


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


def get_rabbits(data: LogData):
  data.genome_info()
  i = 0
  for val in data.genome_info():
    i += 1
    print(i)
    print('\n')
  # return [(1, 1, "x", [(0, 1), (2, 3), (4, 5)])]


def is_predator(tag: str) -> bool:
  return tag == "Wolf" or tag == "Bear"


def is_prey(tag: str) -> bool:
  return tag == "Rabbit" or tag == "Deer"
