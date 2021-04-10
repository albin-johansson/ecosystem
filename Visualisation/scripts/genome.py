"""
This module is responsible for visualising changes in the animal populations.
"""

import matplotlib.pyplot as plot
from pathlib import Path
from logdata import *


def tmp(data: LogData, directory: Path):
    #TODO: example does not contain hungerRates.
    """
    Produces a stackplot of how the animals populations changed over the course of the simulation.

    :param data: the simulation data to read from.
    :param directory:
    :return: the directory to which the plot will be saved.


    times, population_by_tag = create_stackplot_data(data)

    figure, axes = plot.subplots()
    colors = [rabbit_color, deer_color, wolf_color, bear_color]
    axes.stackplot(times, population_by_tag.values(), labels=population_by_tag.keys(), colors=colors)
    axes.legend(loc='lower left')
    axes.set_title("Population size changes")
    axes.set_xlabel("Time (seconds)")
    axes.set_ylabel("Population size")
    axes.set_xlim(0, data.duration_secs())
    axes.set_ylim(0, max(data.initial_total_alive_count(), data.alive_count()) + 20)

    plot.savefig(directory / Path("animal_populations_stackplot.png"))
    plot.close()
    """


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
    tmp(LogData, Path)
