using System;

class Replica
{
  public Uri address { get; set; }

  public Replica(String address)
  {		
    this.address = new Uri(address);
  }
}
