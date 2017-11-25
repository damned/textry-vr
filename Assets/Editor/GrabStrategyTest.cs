using NUnit.Framework;

[TestFixture]
public class GrabStrategyTest
{

  // todo: thoughts from typing tests:
  // already thinking grab strategy should do knobs.add_layer([knobs]); then
  // grab strategy should hand off "knob grabbed" to whatever's going to manage
  //     what the next layer of knobs should actually be:  a text predicter?

  [Test]
  public void creates_new_knobs_layer_when_knob_grabbed()
  {
    // var strategy = new GrabStrategy()

  }
}