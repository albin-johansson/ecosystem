import json


# A wrapper around simulation data obtained from a JSON file
class LogData:
  data = {}

  def __init__(self, file_path) -> None:
    file = open(file_path)
    self.data = json.load(file)
    file.close()

  def duration_ms(self) -> int:
    return self.data["duration"]

  def duration_secs(self) -> int:
    return self.data["duration"] / 1_000

  def initial_total_alive_count(self) -> int:
    return self.data["initialAliveCount"]

  def initial_prey_count(self) -> int:
    return self.data["initialAlivePreyCount"]

  def initial_predator_count(self) -> int:
    return self.data["initialAlivePredatorCount"]

  def deaths(self):
    return self.data["deaths"]

  def __getitem__(self, item):  # operator[]
    return self.data[item]
