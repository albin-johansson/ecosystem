import matplotlib.pyplot as plot


def main():
  figure, axes = plot.subplots()

  average_fps_data: dict[int, float] = {500: 237.4,
                                        1000: 221.3,
                                        2000: 121.7,
                                        3000: 68.3,
                                        4000: 45.0,
                                        5000: 32.7,
                                        6000: 23.9,
                                        7000: 17.6}

  min_fps_data: dict[int, float] = {500: 150.1,
                                    1000: 102.7,
                                    2000: 54.1,
                                    3000: 30.6,
                                    4000: 20.2,
                                    5000: 16.8,
                                    6000: 8.9,
                                    7000: 5.5}

  max_fps_data: dict[int, float] = {500: 240.0,
                                    1000: 240.0,
                                    2000: 240.0,
                                    3000: 127.4,
                                    4000: 102.1,
                                    5000: 66.5,
                                    6000: 33.8,
                                    7000: 26.3}

  axes.plot(min_fps_data.keys(), min_fps_data.values(), label="Minimum FPS", color="blue", **{"ls": "-"})
  axes.plot(max_fps_data.keys(), max_fps_data.values(), label="Maximum FPS", color="red", **{"ls": "-"})
  axes.plot(average_fps_data.keys(), average_fps_data.values(), label="Average FPS", color="green", **{"ls": "-"})

  axes.legend(loc="upper right")
  axes.set_xlim(500, 7_000)
  axes.set_ylim(0, 250)
  axes.set_xlabel("Amount of entities")
  axes.set_ylabel("Frames per second (FPS)")
  axes.set_title("Performance degradation in ECS scene")

  plot.savefig("ecs_scene_performance_degradation.png")
  plot.close()


if __name__ == "__main__":
  main()
