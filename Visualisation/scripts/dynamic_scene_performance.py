import matplotlib.pyplot as plot


def main():
  figure, axes = plot.subplots()

  average_fps_data: dict[int, float] = {500: 39.1,
                                        600: 32.2,
                                        700: 27.7,
                                        800: 24.2,
                                        900: 21.2,
                                        1000: 18.8,
                                        1200: 15.6,
                                        1400: 13.0,
                                        1600: 11.2,
                                        1800: 10.0,
                                        2000: 8.9}

  min_fps_data: dict[int, float] = {500: 28.4,
                                    600: 25.2,
                                    700: 25.0,
                                    800: 19.0,
                                    900: 18.4,
                                    1000: 16.6,
                                    1200: 11.2,
                                    1400: 10.0,
                                    1600: 8.9,
                                    1800: 7.9,
                                    2000: 7.3}

  max_fps_data: dict[int, float] = {500: 42.1,
                                    600: 34.6,
                                    700: 29.3,
                                    800: 26.4,
                                    900: 23.4,
                                    1000: 20.6,
                                    1200: 17.0,
                                    1400: 14.8,
                                    1600: 13.0,
                                    1800: 10.9,
                                    2000: 10.4}

  axes.plot(min_fps_data.keys(), min_fps_data.values(), label="Minimum FPS", color="blue", **{"ls": "-"})
  axes.plot(max_fps_data.keys(), max_fps_data.values(), label="Maximum FPS", color="red", **{"ls": "-"})
  axes.plot(average_fps_data.keys(), average_fps_data.values(), label="Average FPS", color="green", **{"ls": "-"})

  axes.legend(loc="upper right")
  axes.set_xlim(500, 2_000)
  axes.set_ylim(0, 70)
  axes.set_xlabel("Amount of game objects")
  axes.set_ylabel("Frames per second (FPS)")
  axes.set_title("Performance degradation in dynamic scene")

  plot.savefig("dynamic_scene_performance_degradation.png")
  plot.close()


if __name__ == "__main__":
  main()
