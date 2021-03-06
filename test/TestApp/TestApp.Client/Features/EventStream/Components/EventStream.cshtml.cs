﻿namespace TestApp.Client.Features.EventStream.Components
{
  using System.Collections.Generic;
  using TestApp.Client.Components;

  public class EventStreamModel : BaseComponent
  {
    public List<string> Events => GetState<EventStreamState>().Events;

  }
}
