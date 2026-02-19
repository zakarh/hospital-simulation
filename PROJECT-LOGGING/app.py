# import sqlite3
from flask import Flask, request
from pathlib import Path
from werkzeug.exceptions import HTTPException
import json
from apscheduler.schedulers.background import BackgroundScheduler
from objects import TaskFromForm
import configparser

FILE_PATH_DIR = Path(__file__).parent
SESSIONS_PATH_DIR = FILE_PATH_DIR.joinpath("sessions")


class App:
    def __init__(self) -> None:
        # self.connection = sqlite3.connect(FILE_PATH_DIR.joinpath("app.db"))
        # self.cursor = self.connection.cursor()
        self.flask_app = Flask(__name__)
        self.scheduler = BackgroundScheduler()
        self.environment_state = {}
        self.resource_state = {}
        self.agent_state = {}
        self.scheduler.start()

    def build(self):
        @self.flask_app.errorhandler(HTTPException)
        def handle_exception(e):
            """Return JSON instead of HTML for HTTP errors."""
            # start with the correct headers and status code from the error
            response = e.get_response()
            # replace the body with JSON
            print(
                {
                    "code": e.code,
                    "name": e.name,
                    "description": e.description,
                    "url": request.full_path,
                }
            )
            return response

        @self.flask_app.route("/log/task", methods=["POST", "GET"])
        def log_task():
            if request.method == "POST":
                # print(request.form)
                session_time = request.form.get("session_time", ".null").replace(
                    ":", "."
                )
                session_guid = request.form.get("session_guid", "null")
                task_time_start = request.form.get("time_start", "null").replace(
                    ":", "."
                )
                task_guid = request.form.get("task_guid", "null")
                location = SESSIONS_PATH_DIR.joinpath(
                    f"{session_time}_{session_guid}/tasks"
                )
                name = f"{task_time_start}_{task_guid}.json"
                self.scheduler.add_job(
                    self.write,
                    "interval",
                    seconds=0,
                    kwargs={
                        "parent_dir": location,
                        "file_name": name,
                        "data": json.dumps(TaskFromForm(request.form)),
                    },
                )

            return str(200)

        @self.flask_app.route("/log/action")
        def log_action():
            return "<p>Hello, World!</p>"

        @self.flask_app.route("/log/event")
        def log_event():
            return "<p>Hello, World!</p>"

        # @self.flask_app.route("/environment/state/agent/<uid>")
        # def agent_state(a_type=""):
        #     return "<p>Hello, World!</p>"

        # @self.flask_app.route("/environment/state/area/<uid>")
        # def environment_state(e_type=""):
        #     return "<p>Hello, World!</p>"

        return self

    def write(self, parent_dir: Path, file_name: str, data):
        parent_dir.mkdir(parents=True, exist_ok=True)
        with open(parent_dir.joinpath(file_name), "w") as f:
            f.write(data)

    def run(self, *args, **kwargs):
        self.flask_app.run(*args, **kwargs)


def main():
    config = configparser.ConfigParser()
    config.read(FILE_PATH_DIR.joinpath("config.ini"))
    app = App()
    app.build().run(
        host=config["FLASK"]["host"],
        port=config["FLASK"]["port"],
        debug=True,
    )


if __name__ == "__main__":
    main()
