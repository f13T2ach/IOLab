from flask import Blueprint, jsonify
import config

server_info_bp = Blueprint('server_info', __name__)

@server_info_bp.route('/api/server', methods=['POST'])
def getServer():
    return jsonify(
        {"name": config.ServerConfig.name, "area": config.ServerConfig.area},
    )
