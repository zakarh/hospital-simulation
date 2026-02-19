using System.Collections;
using System.Collections.Generic;

public enum Action
{
    SelfFeed,
    SelfClean,
    SelfSleep,
    PatientFeed, // When assistance is required
    PatientClean, // When assistance is required
    DisposeWaste, // Waste generated from feeding and cleaning patients. 
    AnalyzePatientConditions,
    TreatPatientConditions,
    WaitOnStandby,
}