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

  def initial_food_count(self) -> int:
    return self.data["initialFoodCount"]

  def initial_total_alive_count(self) -> int:
    return self.data["initialAliveCount"]

  def initial_prey_count(self) -> int:
    return self.data["initialAlivePreyCount"]

  def initial_predator_count(self) -> int:
    return self.data["initialAlivePredatorCount"]

  def deaths(self):
    return self.data["deaths"]

  def food_consumptions(self):
    return self.data["foodConsumptions"]

  def __getitem__(self, item):  # operator[]
    return self.data[item]


def get_population_history(data: LogData, tag: str, initial_count: Amount) -> dict[TimePoint, Amount]:
  """
  Returns the population history for a class of animal, e.g. rabbits or wolves.

  :param data: the data wrapper to read from.
  :param tag: the tag associated with the animals to obtain the history of.
  :param initial_count: the initial amount of animals.
  :return: a dictionary that maps time points (in seconds) to the associated population size.
  """

  history: dict[TimePoint, Amount] = {0: initial_count}
  count: Amount = initial_count

  for death in data.deaths():
    if death["tag"] == tag:
      count = count - 1

      time: TimePoint = death["time"] / 1_000
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

  for consumption in data.food_consumptions():
    time: TimePoint = consumption["time"] / 1_000

    food_count = food_count - 1
    food_history[time] = food_count

  food_history[data.duration_secs()] = food_count

  return food_history
