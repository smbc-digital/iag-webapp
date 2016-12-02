#!/bin/bash
running_locally() {
  if [[ -n "$SNAP_PIPELINE_COUNTER"  ]] || [[ -n "$GO_PIPELINE_LABEL" ]] ; then
    echo false
  else
    echo true
  fi
}

use_proxy() {
  if [[ -n "$SNAP_PIPELINE_COUNTER"  ]] ; then
    echo false
  else
    echo true
  fi
}
