import json


class Task(dict):
    def __init__(self, data={}):
        super().__init__()
        self["time_start"] = data.get("time_start", None)
        self["time_end"] = data.get("time_end", None)
        self["action_type"] = data.get("action_type", None)
        self["total_time"] = data.get("total_time", 0)
        self["distance_traveled"] = data.get("distance_traveled", 0)
        self["path_taken"] = data.get("path_taken", [])


class TaskFromForm(Task):
    def __init__(self, form={}):
        super().__init__()
        self.from_form(form)

    def from_form(self, form={}):
        self["task_guid"] = form.get("task_guid", None)
        self["time_start"] = form.get("time_start", None)
        self["time_end"] = form.get("time_end", None)
        self["action_type"] = form.get("action_type", None)
        self["total_time"] = form.get("total_time", 0)
        self["distance_traveled"] = form.get("distance_traveled", 0)
        self["path_taken"] = json.loads(form.get("path_taken", []))
