namespace CharacterGenerator.Models
{
    abstract class PlayerClass
    {
        public int ClassHitPoints { get; set; }
        public Stats ClassStats { get; set; }
        public Subclass Subclass { get; set; }
    }
  

    // class Fighter : PlayerClass
    // {
    //     public override ClassHitPoints = 10;
    // }
}