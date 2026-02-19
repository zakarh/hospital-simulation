using System.Collections;
using System.Collections.Generic;

public class Condition {
    public string name;
    public Level severity;
    public bool analyzed;
    public bool treated;
    public int last_treated;
    // Worsening progression of condition [0, 100]. 100 == untreatable.
    public int value;
}
