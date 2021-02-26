from pathlib import Path

import matplotlib.pyplot as plot

from logdata import LogData


# Returns the prey and predator history as two dictionaries with (time -> count)
def load_animal_history(data: LogData) -> tuple[dict[int, int], dict[int, int]]:
  initial_prey_count: int = data.initial_prey_count()
  initial_predator_count: int = data.initial_predator_count()

  prey_history: dict[int, int] = {0: initial_prey_count}
  predator_history: dict[int, int] = {0: initial_predator_count}

  prey_count: int = initial_prey_count
  predator_count: int = initial_predator_count

  for death in data.deaths():
    time: int = death['time'] / 1_000
    tag: str = death['tag']

    if tag == 'Prey':
      prey_count = prey_count - 1
      prey_history[time] = prey_count

    elif tag == 'Predator':
      predator_count = predator_count - 1
      predator_history[time] = predator_count

  prey_history[data.duration_secs()] = prey_count
  predator_history[data.duration_secs()] = predator_count

  return prey_history, predator_history


def visualise_animal_population_sizes(data: LogData, directory: Path):
  prey_history, predator_history = load_animal_history(data)

  figure, axes = plot.subplots()

  line_styles = [{'ls': '-'}, {'ls': '--'}, {'ls': ':'}, {'ls': '-.'},
                 {'dashes': [2, 4, 2, 4, 8, 4]}]

  axes.plot(prey_history.keys(), prey_history.values(), label='Prey', color='green', **line_styles[0])
  axes.plot(predator_history.keys(), predator_history.values(), label='Predator', color='red', **line_styles[1])

  axes.legend(loc='upper left')
  axes.set_xlim(0, data.duration_secs())
  axes.set_ylim(0, data.initial_total_alive_count() + 20)
  axes.set_xlabel('Time (seconds)')
  axes.set_ylabel('Population size')

  plot.savefig(directory / Path('animal_population_sizes.png'))
