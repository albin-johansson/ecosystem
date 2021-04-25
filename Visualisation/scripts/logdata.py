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
  def rabbit_hunger_rate(self):
    return self.averageGenomes("rabbitAverageGenomes", "HungerRate")

  def rabbit_hunger_threshold(self):
    return self.averageGenomes("rabbitAverageGenomes", "HungerThreshold")

  def rabbit_thirst_rate(self):
    return self.averageGenomes("rabbitAverageGenomes", "ThirstRate")

  def rabbit_thirst_threshold(self):
    return self.averageGenomes("rabbitAverageGenomes", "ThirstThreshold")

  def rabbit_vision(self):
    return self.averageGenomes("rabbitAverageGenomes", "Vision")

  def rabbit_speed(self):
    return self.averageGenomes("rabbitAverageGenomes", "Speed")

  def rabbit_size_factor(self):
    return self.averageGenomes("rabbitAverageGenomes", "SizeFactor")

  def rabbit_desirability_score(self):
    return self.averageGenomes("rabbitAverageGenomes", "DesirabilityScore")

  def rabbit_gestation_period(self):
    return self.averageGenomes("rabbitAverageGenomes", "GestationPeriod")

  def rabbit_sexual_maturity_time(self):
    return self.averageGenomes("rabbitAverageGenomes", "SexualMaturityTime")

  # Wolf gets
  def wolf_hunger_rate(self):
    return self.averageGenomes("wolfAverageGenomes", "HungerRate")

  def wolf_hunger_threshold(self):
    return self.averageGenomes("wolfAverageGenomes", "HungerThreshold")

  def wolf_thirst_rate(self):
    return self.averageGenomes("wolfAverageGenomes", "ThirstRate")

  def wolf_thirst_threshold(self):
    return self.averageGenomes("wolfAverageGenomes", "ThirstThreshold")

  def wolf_vision(self):
    return self.averageGenomes("wolfAverageGenomes", "Vision")

  def wolf_speed(self):
    return self.averageGenomes("wolfAverageGenomes", "Speed")

  def wolf_size_factor(self):
    return self.averageGenomes("wolfAverageGenomes", "SizeFactor")

  def wolf_desirability_score(self):
    return self.averageGenomes("wolfAverageGenomes", "DesirabilityScore")

  def wolf_gestation_period(self):
    return self.averageGenomes("wolfAverageGenomes", "GestationPeriod")

  def wolf_sexual_maturity_time(self):
    return self.averageGenomes("wolfAverageGenomes", "SexualMaturityTime")

  # Deer gets
  def deer_hunger_rate(self):
    return self.averageGenomes("deerAverageGenomes", "HungerRate")

  def deer_hunger_threshold(self):
    return self.averageGenomes("deerAverageGenomes", "HungerThreshold")

  def deer_thirst_rate(self):
    return self.averageGenomes("deerAverageGenomes", "ThirstRate")

  def deer_thirst_threshold(self):
    return self.averageGenomes("deerAverageGenomes", "ThirstThreshold")

  def deer_vision(self):
    return self.averageGenomes("deerAverageGenomes", "Vision")

  def deer_speed(self):
    return self.averageGenomes("deerAverageGenomes", "Speed")

  def deer_size_factor(self):
    return self.averageGenomes("deerAverageGenomes", "SizeFactor")

  def deer_desirability_score(self):
    return self.averageGenomes("deerAverageGenomes", "DesirabilityScore")

  def deer_gestation_period(self):
    return self.averageGenomes("deerAverageGenomes", "GestationPeriod")

  def deer_sexual_maturity_time(self):
    return self.averageGenomes("deerAverageGenomes", "SexualMaturityTime")

  # Bear gets
  def bear_hunger_rate(self):
    return self.averageGenomes("bearAverageGenomes", "HungerRate")

  def bear_hunger_threshold(self):
    return self.averageGenomes("bearAverageGenomes", "HungerThreshold")

  def bear_thirst_rate(self):
    return self.averageGenomes("bearAverageGenomes", "ThirstRate")

  def bear_thirst_threshold(self):
    return self.averageGenomes("bearAverageGenomes", "ThirstThreshold")

  def bear_vision(self):
    return self.averageGenomes("bearAverageGenomes", "Vision")

  def bear_speed(self):
    return self.averageGenomes("bearAverageGenomes", "Speed")

  def bear_size_factor(self):
    return self.averageGenomes("bearAverageGenomes", "SizeFactor")

  def bear_desirability_score(self):
    return self.averageGenomes("bearAverageGenomes", "DesirabilityScore")

  def bear_gestation_period(self):
    return self.averageGenomes("bearAverageGenomes", "GestationPeriod")

  def bear_sexual_maturity_time(self):
    return self.averageGenomes("bearAverageGenomes", "SexualMaturityTime")

  def box_genomes(self, animal: str, gene: str):
    animal = "rabbitBoxGenomes"
    gene = "HungerRate"
    list = self.data[animal][gene]
    list2 = []
    for v in list:
      list2.append(BoxGenomeEntry(v["entryTime"], v["value"]))
    return list2

    # Rabbit gets

  def rabbit_hunger_rate_box(self):
    return self.averageGenomes("rabbitBoxGenomes", "HungerRate")

  def rabbit_hunger_threshold_box(self):
    return self.box_genomes("rabbitBoxGenomes", "HungerThreshold")

  def rabbit_thirst_rate_box(self):
    return self.box_genomes("rabbitBoxGenomes", "ThirstRate")

  def rabbit_thirst_threshold_box(self):
    return self.box_genomes("rabbitBoxGenomes", "ThirstThreshold")

  def rabbit_vision_box(self):
    return self.box_genomes("rabbitBoxGenomes", "Vision")

  def rabbit_speed_box(self):
    return self.box_genomes("rabbitBoxGenomes", "Speed")

  def rabbit_size_factor_box(self):
    return self.box_genomes("rabbitBoxGenomes", "SizeFactor")

  def rabbit_desirability_score_box(self):
    return self.box_genomes("rabbitBoxGenomes", "DesirabilityScore")

  def rabbit_gestation_period_box(self):
    return self.box_genomes("rabbitBoxGenomes", "GestationPeriod")

  def rabbit_sexual_maturity_time_box(self):
    return self.box_genomes("rabbitBoxGenomes", "SexualMaturityTime")

  # Wolf gets
  def wolf_hunger_rate_box(self):
    return self.box_genomes("wolfBoxGenomes", "HungerRate")

  def wolf_hunger_threshold_box(self):
    return self.box_genomes("wolfBoxGenomes", "HungerThreshold")

  def wolf_thirst_rate_box(self):
    return self.box_genomes("wolfBoxGenomes", "ThirstRate")

  def wolf_thirst_threshold_box(self):
    return self.box_genomes("wolfBoxGenomes", "ThirstThreshold")

  def wolf_vision_box(self):
    return self.box_genomes("wolfBoxGenomes", "Vision")

  def wolf_speed_box(self):
    return self.box_genomes("wolfBoxGenomes", "Speed")

  def wolf_size_factor_box(self):
    return self.box_genomes("wolfBoxGenomes", "SizeFactor")

  def wolf_desirability_score_box(self):
    return self.box_genomes("wolfBoxGenomes", "DesirabilityScore")

  def wolf_gestation_period_box(self):
    return self.box_genomes("wolfBoxGenomes", "GestationPeriod")

  def wolf_sexual_maturity_time_box(self):
    return self.box_genomes("wolfBoxGenomes", "SexualMaturityTime")

  # Deer gets
  def deer_hunger_rate_box(self):
    return self.box_genomes("deerBoxGenomes", "HungerRate")

  def deer_hunger_threshold_box(self):
    return self.box_genomes("deerBoxGenomes", "HungerThreshold")

  def deer_thirst_rate_box(self):
    return self.box_genomes("deerBoxGenomes", "ThirstRate")

  def deer_thirst_threshold_box(self):
    return self.box_genomes("deerBoxGenomes", "ThirstThreshold")

  def deer_vision_box(self):
    return self.box_genomes("deerBoxGenomes", "Vision")

  def deer_speed_box(self):
    return self.box_genomes("deerBoxGenomes", "Speed")

  def deer_size_factor_box(self):
    return self.box_genomes("deerBoxGenomes", "SizeFactor")

  def deer_desirability_score_box(self):
    return self.box_genomes("deerBoxGenomes", "DesirabilityScore")

  def deer_gestation_period_box(self):
    return self.box_genomes("deerBoxGenomes", "GestationPeriod")

  def deer_sexual_maturity_time_box(self):
    return self.box_genomes("deerBoxGenomes", "SexualMaturityTime")

  # Bear gets
  def bear_hunger_rate_box(self):
    return self.box_genomes("bearBoxGenomes", "HungerRate")

  def bear_hunger_threshold_box(self):
    return self.box_genomes("bearBoxGenomes", "HungerThreshold")

  def bear_thirst_rate_box(self):
    return self.box_genomes("bearBoxGenomes", "ThirstRate")

  def bear_thirst_threshold_box(self):
    return self.box_genomes("bearBoxGenomes", "ThirstThreshold")

  def bear_vision_box(self):
    return self.box_genomes("bearBoxGenomes", "Vision")

  def bear_speed_box(self):
    return self.box_genomes("bearBoxGenomes", "Speed")

  def bear_size_factor_box(self):
    return self.box_genomes("bearBoxGenomes", "SizeFactor")

  def bear_desirability_score_box(self):
    return self.box_genomes("bearBoxGenomes", "DesirabilityScore")

  def bear_gestation_period_box(self):
    return self.box_genomes("bearBoxGenomes", "GestationPeriod")

  def bear_sexual_maturity_time_box(self):
    return self.box_genomes("bearBoxGenomes", "SexualMaturityTime")


class BoxGenomeEntry:
  time = 0
  value = []

  def __init__(self, time: int, value: []):
    self.time = time
    self.value = value


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
