"""
This module provides utilities related to handling the simulation log data.
"""

import json

TimePoint = int
Amount = int


class BoxGenomeEntry:
  time: int = 0
  values: list[float] = []

  def merge(self, other):
    self.values.append(other.values)

  def __init__(self, time: int, values: list[float]):
    self.time = time
    self.values = values


class AverageGenomeEntry:
  time: int = 0
  value: float = 0

  def __init__(self, time: int, value: float):
    self.time = time
    self.value = value


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

  def average_genomes(self, animal: str, gene: str) -> list[AverageGenomeEntry]:
    obj = self.data[animal][gene]
    result: list[AverageGenomeEntry] = []

    for entry in obj:
      result.append(AverageGenomeEntry(entry["entryTime"], entry["value"]))

    return result

  def box_genomes(self, animal: str, gene: str) -> list[BoxGenomeEntry]:
    data = self.data[animal][gene]

    result = []
    for entry in data:
      result.append(BoxGenomeEntry(entry["entryTime"], entry["value"]))

    return result

  def average_hunger_rate(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "HungerRate")

  def average_hunger_threshold(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "HungerThreshold")

  def average_thirst_rate(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "ThirstRate")

  def average_thirst_threshold(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "ThirstThreshold")

  def average_vision(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "Vision")

  def average_speed(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "Speed")

  def average_size_factor(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "SizeFactor")

  def average_desirability_score(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "DesirabilityScore")

  def average_gestation_period(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "GestationPeriod")

  def average_sexual_maturity_time(self, animal: str) -> list[AverageGenomeEntry]:
    return self.average_genomes(animal + "AverageGenomes", "SexualMaturityTime")

  def box_hunger_rate(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "HungerRate")

  def box_hunger_threshold(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "HungerThreshold")

  def box_thirst_rate(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "ThirstRate")

  def box_thirst_threshold(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "ThirstThreshold")

  def box_vision(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "Vision")

  def box_speed(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "Speed")

  def box_size_factor(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "SizeFactor")

  def box_desirability_score(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "DesirabilityScore")

  def box_gestation_period(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "GestationPeriod")

  def box_sexual_maturity_time(self, animal: str) -> list[BoxGenomeEntry]:
    return self.box_genomes(animal + "BoxGenomes", "SexualMaturityTime")


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
